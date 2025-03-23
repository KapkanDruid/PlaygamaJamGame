using Project.Content.UI;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class BuildingMockPlacer : MonoBehaviour
    {
        private GridPlaceSystem _gridPlaceSystem;
        private CardsPopupView _cardsPopupView;
        private SceneData _sceneData;

        [Inject]
        private void Construct(GridPlaceSystem gridPlaceSystem, SceneData sceneData, CardsPopupView cardsPopupView)
        {
            _gridPlaceSystem = gridPlaceSystem;
            _cardsPopupView = cardsPopupView;
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
                _sceneData.TurretDynamicData[TurretType.VoiceOfTruth].MaxHealth.Value = 100;
            }
        }

        [ContextMenu("PlaceObject")]
        private void PlaceObject()
        {
            _cardsPopupView.Show();
/*            var placeEntity = _turretFactory.Create();

            var placeComponent = placeEntity.ProvideComponent<GridPlaceComponent>();

            if (placeComponent != null)
            {
                _gridPlaceSystem.StartPlacing(placeComponent);

            }*/
        }
    }
}
