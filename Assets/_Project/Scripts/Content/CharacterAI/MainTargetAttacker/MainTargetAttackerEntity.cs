using Cysharp.Threading.Tasks;
using Project.Content.BuildSystem;
using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    public class MainTargetAttackerEntity : CharacterHandler
    {
        [SerializeField] private MainTargetAttackerData _mainTargetAttackerData;

        private bool _isPathInvalid;
        private ClosestTargetSensorFilter _sensorFilter;
        private TargetSensor _sensor;
        private Animator _animator;
        private LevelExperienceController _levelExperience;
        private IEntity _blockingEntity;
        private IEntity _targetEntity;
        private FloatingTextHandler _textHandler;
        private Transform _targetTransform;
        private PauseHandler _pauseHandler;
        private AnimatorStateInfo _pausedAnimatorState;

        public Transform TargetTransform => _targetTransform;
        public IEntity TargetEntity => _targetEntity;
        public IEntity BlockingEntity => _blockingEntity;
        public ICharacterData MainTargetAttackerData => _mainTargetAttackerData;

        public bool PathInvalid => _isPathInvalid;

        public event Action PathBlocked;

        public class Factory : PlaceholderFactory<MainTargetAttackerEntity> 
        {
            public readonly MainTargetAttackerType Type;

            public Factory(MainTargetAttackerType type) : base()
            {
                Type = type;
            }
        }

        [Inject]
        public void Construct(EnemyDeadHandler enemyDeadHandler,
                              Animator animator,
                              LevelExperienceController levelExperience,
                              FloatingTextHandler textHandler,
                              PauseHandler pauseHandler)
        {
            _mainTargetAttackerData.ThisEntity = this;
            _enemyDeadHandler = enemyDeadHandler;
            _animator = animator;
            _levelExperience = levelExperience;
            _textHandler = textHandler;
            _pauseHandler = pauseHandler;

            ResetData();
            _enemyDeadHandler.OnDeath += DropExperience;
        }

        public void Initialize()
        {
            _mainTargetAttackerData.Initialize();

            _cancellationToken = this.GetCancellationTokenOnDestroy();
            _sensorFilter = new ClosestTargetSensorFilter(_mainTargetAttackerData.CharacterTransform);

            _sensor = new TargetSensor(_mainTargetAttackerData.SensorData, Color.blue);
        }

        public override T ProvideComponent<T>() where T : class
        {
            if (_mainTargetAttackerData.Flags is T flags)
                return flags;

            if (_healthHandler is T healthHandler)
                return healthHandler;

            if (transform is T characterTransform)
                return characterTransform;

            if (_enemyDeadHandler is T deadHandler)
                return deadHandler;

            return null;
        }

        public void IsPathInvalid(bool isInvalid, IEntity blockingEntity = null)
        {
            _isPathInvalid = isInvalid;
            if (isInvalid)
            {
                PathBlocked?.Invoke();
                _blockingEntity = blockingEntity;
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (_pauseHandler.IsPaused)
            {
                PauseAnimation();
                return;
            }

            ResumeAnimation();

            HandleTarget();

        }

        private void HandleTarget()
        {
            if (_targetTransform == null)
            {
                if (_sensor == null)
                {
                    Debug.Log("Sensor is null");
                }

                if (!_sensor.TryGetTarget(out IEntity entity, out Transform targetTransform, _sensorFilter))
                    return;

                _targetTransform = targetTransform;
                _targetEntity = entity;
            }
            else
            {
                if (_targetTransform.gameObject.activeInHierarchy)
                    return;

                _targetTransform = null;
            }
        }

        private void DropExperience()
        {
            _levelExperience.OnEnemyDied(_mainTargetAttackerData.CharacterTransform.position, _mainTargetAttackerData.ExperiencePoints);
        }

        private void OnEnable()
        {
            ResetData();
            _animator.Rebind();
            _animator.Update(0f);
            _enemyDeadHandler.Reset();
            _healthHandler.Reset();
        }

        private void PauseAnimation()
        {
            if (_animator.speed != 0)
            {
                _pausedAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
                _animator.speed = 0;
            }
        }

        private void ResumeAnimation()
        {
            if (_animator.speed == 0)
            {
                _animator.speed = 1;
                _animator.Play(_pausedAnimatorState.fullPathHash, -1, _pausedAnimatorState.normalizedTime);
            }
        }

        private void ResetData()
        {
            _healthHandler = new CharacterHealthHandler(_mainTargetAttackerData.Health, _animator, _enemyDeadHandler);

            if (_mainTargetAttackerData.FloatingText != null)
                _healthHandler.OnDamage += (damage) => _textHandler.ShowText(_mainTargetAttackerData.FloatingText, _mainTargetAttackerData.CharacterTransform.position, damage.ToString());
        }

        private void OnDestroy()
        {
            _enemyDeadHandler.OnDeath -= DropExperience;
        }
    }
}