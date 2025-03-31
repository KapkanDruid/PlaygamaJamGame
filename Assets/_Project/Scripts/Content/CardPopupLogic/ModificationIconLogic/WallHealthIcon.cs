using Project.Architecture;
using Project.Content.BuildSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Content.UI
{
    public class WallHealthIcon : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _baseText;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private TextMeshProUGUI _currentText;

        private SceneData _sceneData;
        private WallDynamicData _dynamicData;
        private WallConfig _config;

        [Inject]
        private void Construct(SceneData sceneData)
        {
            _sceneData = sceneData;
            MainSceneBootstrap.OnServicesInitialized += Initialize;
        }

        private void Initialize()
        {
            _dynamicData = _sceneData.WallDynamicData;
            _config = _dynamicData.Config;
        }

        private void OnEnable()
        {
            if (_dynamicData == null)
                return;

            _baseText.text = _config.MaxHealth.ToString();
            _bonusText.text = (_dynamicData.BuildingMaxHealth.Value - _config.MaxHealth).ToString();
            _currentText.text = _dynamicData.BuildingMaxHealth.Value.ToString();
        }

        private void OnDestroy()
        {
            MainSceneBootstrap.OnServicesInitialized -= Initialize;
        }
    }
}
