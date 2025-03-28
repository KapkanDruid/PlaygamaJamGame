using Project.Architecture;
using Project.Content.BuildSystem;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Infantryman
{
    public class InfantrymanEntity : CharacterHandler, IPatrolling, IInitializable
    {
        [SerializeField] private InfantrymanData _infantrymanData;
        [SerializeField] private SpriteRenderer _levelSpriteRenderer;
        private ClosestTargetSensorFilter _sensorFilter;
        private TargetSensor _sensor;
        private Transform _targetTransform;
        private Transform _flagTransform;
        private Animator _animator;
        private PauseHandler _pauseHandler;
        private float _patrolRadius;
        private FloatingTextHandler _textHandler;
        private EntityCommander _entityCommander;
        private AudioController _audioController;
        public IAllyEntityData InfantrymanData => _infantrymanData;
        public Transform TargetTransform => _targetTransform;
        public Transform FlagTransform => _flagTransform;
        public float PatrolRadius => _patrolRadius;

        public class Factory : PlaceholderFactory<InfantrymanEntity>
        {
            public readonly AllyEntityType Type;

            public Factory(AllyEntityType type) : base()
            {
                Type = type;
            }
        }

        [Inject]
        public void Construct(EnemyDeadHandler enemyDeadHandler,
                              Animator animator,
                              PauseHandler pauseHandler,
                              FloatingTextHandler textHandler,
                              EntityCommander entityCommander, 
                              AudioController audioController)
        {
            _infantrymanData.ThisEntity = this;
            _enemyDeadHandler = enemyDeadHandler;
            _animator = animator;
            _pauseHandler = pauseHandler;
            _textHandler = textHandler;
            _entityCommander = entityCommander;
            _audioController = audioController;

            MainSceneBootstrap.OnServicesInitialized += OnSceneInitialized;
            ResetData();
        }

        public void Initialize()
        {
            _infantrymanData.Initialize();

            _sensorFilter = new ClosestTargetSensorFilter(_infantrymanData.EntityTransform);

            _sensor = new TargetSensor(_infantrymanData.SensorData, Color.blue);
        }

        public void Prepare(InfantrymanSpawnData spawnData)
        {
            _infantrymanData.UpdateData(spawnData);
            ResetData();
            _animator.Rebind();
            _animator.Update(0f);
            _enemyDeadHandler.Reset();
            _healthHandler.Reset();
            UpdateLevelSprite();

            if (_infantrymanData.BornSoundEffect != null && _audioController != null)
                _audioController.PlayOneShot(_infantrymanData.BornSoundEffect);
        }

        private void OnSceneInitialized()
        {
            if (_entityCommander != null)
                _entityCommander.AddEntity(this);
        }

        public override T ProvideComponent<T>() where T : class
        {
            if (_infantrymanData.Flags is T flags)
                return flags;

            if (_healthHandler is T healthHandler)
                return healthHandler;

            if (transform is T characterTransform)
                return characterTransform;

            if (_enemyDeadHandler is T deadHandler)
                return deadHandler;
            
            if (_infantrymanData.Collider is T collider)
                return collider;

            if (this is T thisObject)
                return thisObject;

            return null;
        }

        public void SetFlag(Transform flag)
        {
            _flagTransform = flag;

            var flagComponent = flag.GetComponent<DefensiveFlag>();

            if (flagComponent == null)
                return;

            _patrolRadius = flagComponent.Coverage;
        }

        private void Update()
        {
            if (_pauseHandler.IsPaused)
                return;

            HandleTarget();
        }


        private void ResetData()
        {
            _healthHandler = new CharacterHealthHandler(_infantrymanData.Health, _animator, _enemyDeadHandler);

            if (_infantrymanData.FloatingText != null)
                _healthHandler.OnDamage += (damage) => _textHandler.ShowText(_infantrymanData.FloatingText, _infantrymanData.EntityTransform.position, damage.ToString());
        }

        private void HandleTarget()
        {
            if (_targetTransform == null)
            {
                if (!_sensor.TryGetTarget(out IEntity entity, out Transform targetTransform, _sensorFilter))
                    return;

                _targetTransform = targetTransform;
            }
            else
            {
                if (_targetTransform.gameObject.activeInHierarchy)
                    return;

                _targetTransform = null;
            }
        }

        private void UpdateLevelSprite()
        {
            if (_levelSpriteRenderer == null)
                return;

            _levelSpriteRenderer.sprite = _infantrymanData.UpgradeSprite;
            int level = _infantrymanData.LevelUpgrade;
            Color color = _infantrymanData.GetColorForLevel(level);
            _levelSpriteRenderer.color = color;
        }

        private void OnDisable()
        {
            MainSceneBootstrap.OnServicesInitialized -= OnSceneInitialized;
        }
    }
}

