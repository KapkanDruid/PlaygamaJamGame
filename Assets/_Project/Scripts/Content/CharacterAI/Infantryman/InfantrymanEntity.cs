using Project.Content.BuildSystem;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Infantryman
{
    public class InfantrymanEntity : CharacterHandler
    {
        [SerializeField] private InfantrymanData _infantrymanData;
        [SerializeField] private Transform _flagTransform;

        private object[] _components;
        private TargetSensor _sensor;
        private Transform _targetTransform;
        private Animator _animator;

        public IAllyEntityData InfantrymanData => _infantrymanData;
        public Transform TargetTransform => _targetTransform;
        public Transform FlagTransform => _flagTransform;

        public class Factory : PlaceholderFactory<InfantrymanEntity> { }

        [Inject]
        public void Construct(EnemyDeadHandler enemyDeadHandler, Animator animator)
        {
            _infantrymanData.ThisEntity = this;
            _enemyDeadHandler = enemyDeadHandler;
            _animator = animator;

            List<object> components = new();

            components.Add(_healthHandler);
            components.Add(_enemyDeadHandler);
            components.Add(_infantrymanData.Flags);
            components.Add(this);

            _components = components.ToArray();
        }

        public override T ProvideComponent<T>() where T : class
        {
            for (int i = 0; i < _components.Length; i++)
            {
                object component = _components[i];
                if (component is T)
                    return component as T;
            }

            return null;
        }

        private void Update()
        {
            HandleTarget();
        }

        private void OnEnable()
        {
            ResetData();
            _animator.Rebind();
            _animator.Update(0f);
            _enemyDeadHandler.Reset();
            _healthHandler.Reset();
        }

        private void ResetData()
        {
            _healthHandler = new CharacterHealthHandler(_infantrymanData.Health, _animator, _enemyDeadHandler);
        }

        private void Start()
        {
            _infantrymanData.Initialize();
            _sensor = new TargetSensor(_infantrymanData.SensorData, Color.blue);
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

