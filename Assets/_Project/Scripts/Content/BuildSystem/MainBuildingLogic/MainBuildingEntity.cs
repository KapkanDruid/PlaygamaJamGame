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

        private object[] _components;

        public MainBuildingData Data => _data;

        public class Factory : PlaceholderFactory<MainBuildingEntity> { }

        [Inject]
        private void Construct(GridPlaceComponent placeComponent, BuildingHealthComponent healthHandler)
        {
            List<object> components = new();

            _healthHandler = healthHandler;
            _placeComponent = placeComponent;

            components.Add(_placeComponent);
            components.Add(_healthHandler);
            components.Add(_data.Flags);
            components.Add(this);

            _components = components.ToArray();
        }

        private void Start()
        {
            _healthHandler.Initialize();
            _placeComponent.Initialize();

            _healthHandler.OnDead += DestroyThisAsync;
        }

        private async void DestroyThisAsync()
        {
            await _placeComponent.ReleaseAsync();

            Destroy(this);
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
    }
}
