using Cysharp.Threading.Tasks;
using Project.Content.BuildSystem;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerEntity : CharacterHandler, IGizmosDrawer
    {
        [SerializeField] private DestroyerData _destroyerData;

        private ClosestTargetSensorFilter _sensorFilter;
        private TargetSensor _sensor;
        private LevelExperienceController _levelExperience;
        private Animator _animator;
        private FloatingTextHandler _textHandler;
        private Transform _targetTransform;
        private IEntity _targetEntity;
        private PauseHandler _pauseHandler;
        private AnimatorStateInfo _pausedAnimatorState;
        private IAttackerData _attackerData;

        public Transform TargetTransform => _targetTransform;
        public IEntity TargetEntity => _targetEntity;
        public ICharacterData DestroyerData => _destroyerData;


        public class Factory : PlaceholderFactory<DestroyerEntity>
        {
            public readonly DestroyerType Type;

            public Factory(DestroyerType type) : base()
            {
                Type = type;
            }
        }

        [Inject]
        public void Construct(EnemyDeadHandler enemyDeadHandler,
                              Animator animator,
                              LevelExperienceController levelExperience,
                              FloatingTextHandler textHandler,
                              PauseHandler pauseHandler,
                              IAttackerData attackerData)
        {
            _destroyerData.ThisEntity = this;
            _animator = animator;
            _enemyDeadHandler = enemyDeadHandler;
            _levelExperience = levelExperience;
            _textHandler = textHandler;
            _pauseHandler = pauseHandler;
            _attackerData = attackerData;

            ResetData();
            _enemyDeadHandler.OnDeath += DropExperience;
            _enemyDeadHandler.OnDeath += Death;
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _destroyerData.Initialize();

            _cancellationToken = this.GetCancellationTokenOnDestroy();
            _sensorFilter = new ClosestTargetSensorFilter(_destroyerData.CharacterTransform);

            _sensor = new TargetSensor(_destroyerData.SensorData, Color.blue);
        }

        public override T ProvideComponent<T>() where T : class
        {
            if (_destroyerData.Flags is T flags)
                return flags;

            if (_healthHandler is T healthHandler)
                return healthHandler;

            if (transform is T characterTransform)
                return characterTransform;

            if (_enemyDeadHandler is T deadHandler)
                return deadHandler;

            return null;
        }

        private void DropExperience()
        {
            _levelExperience.OnEnemyDied(_destroyerData.CharacterTransform.position, _destroyerData.ExperiencePoints);
        }

        private void Death()
        {
            if (_destroyerData.Collider != null)
                _destroyerData.Collider.enabled = false;
        }

        private void OnEnable()
        {
            ResetData();
            _animator.Rebind();
            _animator.Update(0f);
            _enemyDeadHandler.Reset();
            _healthHandler.Reset();
            _targetTransform = null;
            if (_destroyerData.Collider != null)
                _destroyerData.Collider.enabled = true;
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

        private void ResetData()
        {
            _healthHandler = new CharacterHealthHandler(_destroyerData.Health, _animator, _enemyDeadHandler);

            if (_destroyerData.FloatingText != null)
                _healthHandler.OnDamage += (damage) => _textHandler.ShowText(_destroyerData.FloatingText, _destroyerData.CharacterTransform.position, damage.ToString());
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

        private void OnDestroy()
        {
            _enemyDeadHandler.OnDeath -= DropExperience;
            _enemyDeadHandler.OnDeath -= Death;
        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)_destroyerData.SensorData.SensorOrigin.position + _attackerData.HitColliderOffset, _attackerData.HitColliderSize);
#endif
        }
    }
}

