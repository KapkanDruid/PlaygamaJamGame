using Zenject;

namespace Project.Content.BuildSystem
{
    public class BuildingSpawner
    {
        private GridPlaceSystem _gridPlaceSystem;

        public BuildingSpawner(GridPlaceSystem gridPlaceSystem)
        {
            _gridPlaceSystem = gridPlaceSystem;
        }

        public void Spawn<T>(PlaceholderFactory<T> factory) where T : IEntity
        {
            var placeEntity = factory.Create();
            var placeComponent = placeEntity.ProvideComponent<GridPlaceComponent>();

            if (placeComponent != null)
            {
                _gridPlaceSystem.StartPlacing(placeComponent);
            }
        }
    }
}
