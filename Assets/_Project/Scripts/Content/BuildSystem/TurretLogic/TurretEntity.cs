using Project.Architecture;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class TurretEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private TurretData _data;

        private BuildingHealthComponent _healthHandler;
        private GridPlaceComponent _placeComponent;
        private GridPlaceSystem _placeSystem;
        private TurretAttackComponent _attackComponent;

        private bool _isRuntimeCreated = true;
        private object[] _components;

        public TurretData Data => _data; 

        public class Factory : PlaceholderFactory<TurretDynamicData, TurretEntity> { }

        [Inject]
        private void Construct(GridPlaceComponent placeComponent,
                               BuildingHealthComponent healthHandler,
                               GridPlaceSystem placeSystem,
                               TurretAttackComponent attackComponent, TurretDynamicData turretDynamicData)
        {
            List<object> components = new();
            _data.Construct(turretDynamicData, this);

            _healthHandler = healthHandler;
            _placeComponent = placeComponent;
            _placeSystem = placeSystem;
            _attackComponent = attackComponent;

            components.Add(_attackComponent);
            components.Add(_placeComponent);
            components.Add(_healthHandler);
            components.Add(_data.Flags);
            components.Add(this);

            _components = components.ToArray();

            _placeComponent.OnPlaced += OnEntityPlaced;
            MainSceneBootstrap.OnServicesInitialized += OnSceneInitialized;
        }

        private void OnSceneInitialized()
        {
            _isRuntimeCreated = false;
        }

        private void Start()
        {
            _data.Initialize();
            _healthHandler.Initialize();
            _placeComponent.Initialize();

            _healthHandler.OnDead += DestroyThisAsync;

            if (_isRuntimeCreated)
                return;

            _placeSystem.PLaceOnGrid(_placeComponent);
        }

        private void OnEntityPlaced()
        {
            _attackComponent.Initialize();
        }

        private async void DestroyThisAsync()
        {
            await _placeComponent.ReleaseAsync();

            Destroy(gameObject);
        }

        public T ProvideComponent<T>() where T : class
        {
            for (int i = 0; i < _components.Length; i++)
            {
                object component = _components[i];
                if (component is T)
                    return component as T;
            }

            return null;
        }

        private void OnDestroy()
        {
            _placeComponent.OnPlaced -= OnEntityPlaced;
            MainSceneBootstrap.OnServicesInitialized -= OnSceneInitialized;
        }
    }
}