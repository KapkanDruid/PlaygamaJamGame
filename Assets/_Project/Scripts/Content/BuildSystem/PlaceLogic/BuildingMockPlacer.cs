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
        private LevelExperienceController _levelExperienceHandler;

        [Inject]
        private void Construct(GridPlaceSystem gridPlaceSystem, SceneData sceneData, CardsPopupView cardsPopupView, LevelExperienceController levelExperienceHandler)
        {
            _gridPlaceSystem = gridPlaceSystem;
            _cardsPopupView = cardsPopupView;
            _sceneData = sceneData;
            _levelExperienceHandler = levelExperienceHandler;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlaceObject();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _sceneData.TurretDynamicData[TurretType.VoiceOfTruth].MaxHealth.Value = 100;
            }
#endif
        }

        [ContextMenu("PlaceObject")]
        private void PlaceObject()
        {
            _levelExperienceHandler.OnEnemyDied(new Vector2(Random.Range(10,-10), Random.Range(5, -5)), 5);
        }
    }
}
