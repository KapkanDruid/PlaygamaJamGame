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
        [SerializeField] private MainBuildingData _data;

        private BuildingHealthComponent _healthHandler;
        private GridPlaceComponent _placeComponent;
        private GridPlaceSystem _placeSystem;
        private WinLoseHandler _winLoseHandler;

        private bool _isRuntimeCreated = true;
        private object[] _components;

        public MainBuildingData Data => _data;

        public class Factory : PlaceholderFactory<MainBuildingEntity> { }

        [Inject]
        private void Construct(GridPlaceComponent placeComponent, BuildingHealthComponent healthHandler, GridPlaceSystem placeSystem, WinLoseHandler winLoseHandler)
        {
            List<object> components = new();

            _healthHandler = healthHandler;
            _placeComponent = placeComponent;
            _placeSystem = placeSystem;
            _winLoseHandler = winLoseHandler;

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
            _data.Initialize();
            _healthHandler.Initialize();
            _placeComponent.Initialize();

            _healthHandler.OnDead += DestroyThisAsync;
            _healthHandler.OnDead += Defeat;

            if (_isRuntimeCreated)
                return;

            _placeSystem.PLaceOnGrid(_placeComponent);
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
            MainSceneBootstrap.OnServicesInitialized -= OnSceneInitialized;
        }
    }
}
