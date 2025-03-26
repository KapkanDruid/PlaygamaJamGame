using Project.Architecture;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class WallEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private WallData _data;
        private WallDynamicData _dynamicData;

        private BuildingHealthComponent _healthHandler;
        private GridPlaceComponent _placeComponent;
        private GridPlaceSystem _placeSystem;
        private SceneData _sceneData;
        private UpgradeEffectController _upgradeEffectController;

        private bool _isRuntimeCreated = true;
        private object[] _components;

        public WallData Data => _data;

        public class Factory : PlaceholderFactory<WallEntity> { }

        [Inject]
        private void Construct(GridPlaceComponent placeComponent, BuildingHealthComponent healthHandler, GridPlaceSystem placeSystem, SceneData sceneData, UpgradeEffectController upgradeEffectController)
        {
            List<object> components = new();

            _sceneData = sceneData;
            _healthHandler = healthHandler;
            _placeComponent = placeComponent;
            _placeSystem = placeSystem;
            _upgradeEffectController = upgradeEffectController;

            components.Add(_placeComponent);
            components.Add(_healthHandler);
            components.Add(_data.Flags);
            components.Add(this);

            _components = components.ToArray();

            MainSceneBootstrap.OnServicesInitialized += OnSceneInitialized;
        }

        private void OnSceneInitialized()
        {
            _isRuntimeCreated = false;
        }

        private void Start()
        {
            _dynamicData = _sceneData.WallDynamicData;

            _data.Construct(_dynamicData);
            _healthHandler.Initialize();
            _placeComponent.Initialize();

            _healthHandler.OnDead += DestroyThisAsync;
            _dynamicData.OnDataUpdate += OnDataUpdate;

            if (_isRuntimeCreated)
                return;

            _placeSystem.PLaceOnGrid(_placeComponent);
        }

        private void OnDataUpdate()
        {
            _upgradeEffectController.PlaySingleEffect(transform.position);
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
            _dynamicData.OnDataUpdate -= OnDataUpdate;
        }
    }
}
