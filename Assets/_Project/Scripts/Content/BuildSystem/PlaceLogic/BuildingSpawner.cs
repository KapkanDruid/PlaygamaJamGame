using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class BuildingSpawner
    {
        private GridPlaceSystem _gridPlaceSystem;
        private Queue<IEntity> _gridPlaceComponents = new Queue<IEntity>();
        private bool _isPlacing;

        public BuildingSpawner(GridPlaceSystem gridPlaceSystem)
        {
            _gridPlaceSystem = gridPlaceSystem;
            _gridPlaceSystem.OnPlaceComponentPlaced += OnBuildingPlaced;
        }

        public void Spawn<T>(PlaceholderFactory<T> factory) where T : IEntity
        {
            var placeEntity = factory.Create();

            placeEntity.ProvideComponent<MonoBehaviour>().gameObject.SetActive(false);

            _gridPlaceComponents.Enqueue(placeEntity);

            PlaceBuilding();
        }

        private void PlaceBuilding()
        {
            if (_isPlacing)
                return;

            _isPlacing = true;
            var placeEntity = _gridPlaceComponents.Dequeue();

            var placeComponent = placeEntity.ProvideComponent<GridPlaceComponent>();

            if (placeComponent != null)
            {
                placeEntity.ProvideComponent<MonoBehaviour>().gameObject.SetActive(true);

                _gridPlaceSystem.StartPlacing(placeComponent);
            }
            else
            {
                Debug.LogError("[BuildingSpawner] Failed to spawn building, entity dos not have GridPlaceComponent");
            }
        }

        private void OnBuildingPlaced()
        {
            if (_gridPlaceComponents.Count > 0)
            {
                _isPlacing = false;
                PlaceBuilding();
            }
            else
                _isPlacing = false;

        }
    }
}
