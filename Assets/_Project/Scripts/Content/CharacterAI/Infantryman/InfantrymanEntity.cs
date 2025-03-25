using Project.Content.BuildSystem;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Infantryman
{
    public class InfantrymanEntity : CharacterHandler, IPatrolling, IInitializable
    {
        [SerializeField] private InfantrymanData _infantrymanData;

        private TargetSensor _sensor;
        private Transform _targetTransform;
        private Transform _flagTransform;
        private Animator _animator;
        private PauseHandler _pauseHandler;
        private float _patrolRadius;

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
        public void Construct(EnemyDeadHandler enemyDeadHandler, Animator animator, PauseHandler pauseHandler)
        {
            _infantrymanData.ThisEntity = this;
            _enemyDeadHandler = enemyDeadHandler;
            _animator = animator;
            _pauseHandler = pauseHandler;

            ResetData();
        }

        public void Initialize()
        {
            _infantrymanData.Initialize();
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

        private void OnEnable()
        {
            /*            ResetData();
                        _animator.Rebind();
                        _animator.Update(0f);
                        _enemyDeadHandler.Reset();
                        _healthHandler.Reset();*/
        }

        private void ResetData()
        {
            _healthHandler = new CharacterHealthHandler(_infantrymanData.Health, _animator, _enemyDeadHandler);
        }

        private void Start()
        {
            /*            _infantrymanData.Initialize();
                        _sensor = new TargetSensor(_infantrymanData.SensorData, Color.blue);*/
        }

        private void HandleTarget()
        {
            if (_targetTransform == null)
            {
                if (!_sensor.TryGetTarget(out IEntity entity, out Transform targetTransform))
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
    }
}

