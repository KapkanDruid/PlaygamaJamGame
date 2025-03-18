using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class BuildingMockPlacer : MonoBehaviour
    {
        private GridPlaceSystem _gridPlaceSystem;
        private MainBuildingEntity.Factory _buildingEntityFactory;

        [Inject]
        private void Construct(GridPlaceSystem gridPlaceSystem, MainBuildingEntity.Factory factory)
        {
            _gridPlaceSystem = gridPlaceSystem;
            _buildingEntityFactory = factory;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlaceObject();
            }
        }

        [ContextMenu("PlaceObject")]
        private void PlaceObject()
        {
            var placeEntity = _buildingEntityFactory.Create();

            var placeComponent = placeEntity.ProvideComponent<GridPlaceComponent>();

            if (placeComponent != null)
            {
                _gridPlaceSystem.StartPlacing(placeComponent);

            }
        }
    }
}
