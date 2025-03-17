using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class MainBuildingEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private MainBuildingData _data;

        private object[] _components;

        public MainBuildingData Data => _data;

        public class Factory : PlaceholderFactory<MainBuildingEntity> { }

        [Inject]
        private void Construct(GridPlaceComponent gridPlaceComponent)
        {
            List<object> components = new();

            components.Add(gridPlaceComponent);

            _components = components.ToArray();
        }

        public T ProvideComponent<T>() where T : class
        {
            foreach (var component in _components)
            {
                if (component is T)
                    return component as T;
            }

            return null;
        }
    }
}
