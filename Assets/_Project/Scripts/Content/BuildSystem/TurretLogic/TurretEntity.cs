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

        private bool _isRuntimeCreated;
        private object[] _components;

        public TurretData Data => _data; 

        public class Factory : PlaceholderFactory<TurretEntity> { }

        [Inject]
        private void Construct(GridPlaceComponent placeComponent, BuildingHealthComponent healthHandler, GridPlaceSystem placeSystem)
        {
            List<object> components = new();

            _healthHandler = healthHandler;
            _placeComponent = placeComponent;
            _placeSystem = placeSystem;

            components.Add(_placeComponent);
            components.Add(_healthHandler);
            components.Add(_data.Flags);
            components.Add(this);

            _components = components.ToArray();

            MainSceneBootstrap.OnServicesInitialized += OnSceneInitialized;
        }

        private void OnSceneInitialized()
        {
            _isRuntimeCreated = true;
        }

        private void Start()
        {
            _healthHandler.Initialize();
            _placeComponent.Initialize();

            _healthHandler.OnDead += DestroyThisAsync;

            if (_isRuntimeCreated)
                return;

            _placeSystem.PLaceOnGrid(_placeComponent);
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
            MainSceneBootstrap.OnServicesInitialized -= OnSceneInitialized;
        }
    }
}