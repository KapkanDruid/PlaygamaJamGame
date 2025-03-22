using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class BuildingMockPlacer : MonoBehaviour
    {
        private GridPlaceSystem _gridPlaceSystem;
        private TurretEntity.Factory _turretFactory;
        private SceneData _sceneData;

        [Inject]
        private void Construct(GridPlaceSystem gridPlaceSystem, [Inject(Id = TurretType.VoiceOfTruth)] TurretEntity.Factory factory, SceneData sceneData)
        {
            _gridPlaceSystem = gridPlaceSystem;
            _turretFactory = factory;
            _sceneData = sceneData;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlaceObject();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _sceneData.TurretDynamicData[TurretType.VoiceOfTruth].ReloadTime -= 5;
            }
        }

        [ContextMenu("PlaceObject")]
        private void PlaceObject()
        {
            var placeEntity = _turretFactory.Create(_sceneData.TurretDynamicData[TurretType.VoiceOfTruth]);

            var placeComponent = placeEntity.ProvideComponent<GridPlaceComponent>();

            if (placeComponent != null)
            {
                _gridPlaceSystem.StartPlacing(placeComponent);

            }
        }
    }
}
