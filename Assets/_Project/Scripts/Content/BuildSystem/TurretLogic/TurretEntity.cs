using Project.Architecture;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class TurretEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private AlertType _alertType;
        [SerializeField] private TurretData _data;
        private TurretDynamicData _dynamicData;

        private BuildingHealthComponent _healthHandler;
        private GridPlaceComponent _placeComponent;
        private GridPlaceSystem _placeSystem;
        private TurretAttackComponent _attackComponent;
        private SceneData _sceneData;
        private UpgradeEffectController _upgradeEffect;
        private AlertController _alertController;

        private bool _isRuntimeCreated = true;
        private object[] _components;

        public TurretData Data => _data; 

        public class Factory : PlaceholderFactory<TurretEntity> 
        {
            public readonly TurretType Type;

            public Factory(TurretType type) : base()
            {
                Type = type;
            }
        }

        [Inject]
        private void Construct(GridPlaceComponent placeComponent,
                               BuildingHealthComponent healthHandler,
                               GridPlaceSystem placeSystem,
                               TurretAttackComponent attackComponent, 
                               SceneData sceneData,
                               UpgradeEffectController upgradeEffect,
                               AlertController alertController)
        {
            List<object> components = new();

            _sceneData = sceneData;
            _healthHandler = healthHandler;
            _placeComponent = placeComponent;
            _placeSystem = placeSystem;
            _attackComponent = attackComponent;
            _upgradeEffect = upgradeEffect;
            _alertController = alertController;

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
            _dynamicData = _sceneData.TurretDynamicData[_data.TurretType];
            _data.Construct(_dynamicData, this);

            _data.Initialize();
            _healthHandler.Initialize();
            _placeComponent.Initialize();

            _healthHandler.OnDead += DestroyThisAsync;
            _healthHandler.OnDead += DestroyAlert;
            _dynamicData.OnDataUpdate += OnDataUpdate;

            if (_isRuntimeCreated)
                return;

            _placeSystem.PLaceOnGrid(_placeComponent);
        }

        private void DestroyAlert()
        {
            _alertController.ShowAlert(_alertType);
        }

        private void OnEntityPlaced()
        {
            _attackComponent.Initialize();
        }

        private void OnDataUpdate()
        {
            _upgradeEffect.PlayTriangleEffect(_data.RotationObject.position, new Vector2(2,2));
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

            if (_placeComponent != null)
                _placeComponent.OnPlaced -= OnEntityPlaced;

            if (_dynamicData != null)
                _dynamicData.OnDataUpdate -= OnDataUpdate;
            _placeComponent.OnPlaced -= OnEntityPlaced;
            _healthHandler.OnDead -= DestroyAlert;
            MainSceneBootstrap.OnServicesInitialized -= OnSceneInitialized;
            _dynamicData.OnDataUpdate -= OnDataUpdate;
        }
    }
}