using Project.Architecture;
using Project.Content.CoreGameLoopLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class MainBuildingEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private AlertType _alertType;
        [SerializeField] private MainBuildingData _data;

        private BuildingHealthComponent _healthHandler;
        private GridPlaceComponent _placeComponent;
        private GridPlaceSystem _placeSystem;
        private WinLoseHandler _winLoseHandler;
        private AlertController _alertController;

        private bool _isRuntimeCreated = true;
        private object[] _components;

        public MainBuildingData Data => _data;

        public class Factory : PlaceholderFactory<MainBuildingEntity> { }

        [Inject]
        private void Construct(GridPlaceComponent placeComponent,
                               BuildingHealthComponent healthHandler,
                               GridPlaceSystem placeSystem,
                               WinLoseHandler winLoseHandler,
                               AlertController alertController)
        {
            List<object> components = new();

            _healthHandler = healthHandler;
            _placeComponent = placeComponent;
            _placeSystem = placeSystem;
            _winLoseHandler = winLoseHandler;
            _alertController = alertController;

            components.Add(_placeComponent);
            components.Add(_healthHandler);
            components.Add(_data.Flags);
            components.Add(this);
            components.Add(_data.Collider);

            _components = components.ToArray();

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
            _healthHandler.OnDead += Defeat;
            _healthHandler.OnTakeDamage += AlertTakeDamage;

            if (_isRuntimeCreated)
                return;

            _placeSystem.PLaceOnGrid(_placeComponent);
        }

        private void AlertTakeDamage()
        {
            _alertController.ShowAlert(_alertType);
        }

        private void Defeat()
        {
            _winLoseHandler.MainBildingDestroyed(true);
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
            _healthHandler.OnDead -= Defeat;
            _healthHandler.OnDead -= DestroyThisAsync;
            _healthHandler.OnTakeDamage -= AlertTakeDamage;
            MainSceneBootstrap.OnServicesInitialized -= OnSceneInitialized;
        }
    }
}
