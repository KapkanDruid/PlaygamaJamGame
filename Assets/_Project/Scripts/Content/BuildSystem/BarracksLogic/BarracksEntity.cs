using Project.Architecture;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Project.Content.BuildSystem
{
    public class BarracksEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private BarracksData _data;
        private BarrackDynamicData _dataDynamic;

        private BuildingHealthComponent _healthHandler;
        private GridPlaceComponent _placeComponent;
        private BarracksSpawnLogic _spawnComponent;
        private GridPlaceSystem _placeSystem;
        private SceneData _sceneData;
        private UpgradeEffectController _upgradeEffect;

        private bool _isRuntimeCreated = true;
        private object[] _components;

        public BarracksData Data => _data;

        public class Factory : PlaceholderFactory<BarracksEntity> 
        {
            public readonly BarracksType Type;

            public Factory(BarracksType type) : base()
            {
                Type = type;
            }
        }

        [Inject]
        private void Construct(GridPlaceComponent placeComponent, BuildingHealthComponent healthHandler, GridPlaceSystem placeSystem, BarracksSpawnLogic spawnComponent, SceneData sceneData, UpgradeEffectController upgradeEffect)
        {
            List<object> components = new();
            _sceneData = sceneData;
            _healthHandler = healthHandler;
            _placeComponent = placeComponent;
            _placeSystem = placeSystem;
            _spawnComponent = spawnComponent;
            _upgradeEffect = upgradeEffect;


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
            _dataDynamic = _sceneData.BarrackDynamicData[_data.BarracksType];

            _data.Construct(_dataDynamic);

            _healthHandler.Initialize();
            _placeComponent.Initialize();
            
            _healthHandler.OnDead += DestroyThisAsync;
            _dataDynamic.OnDataUpdate += OnDataUpdate;

            if (_isRuntimeCreated)
                return;

            _placeSystem.PLaceOnGrid(_placeComponent);
        }

        private void OnEntityPlaced()
        {
            _spawnComponent.Initialize();
        }

        private void OnDataUpdate()
        {
            _upgradeEffect.PlayTriangleEffect(transform.position + new Vector3(0.5f, 0), new Vector2(3, 3));
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
            _placeComponent.OnPlaced += OnEntityPlaced;
            MainSceneBootstrap.OnServicesInitialized -= OnSceneInitialized;
            _dataDynamic.OnDataUpdate -= OnDataUpdate;
        }
    }
}