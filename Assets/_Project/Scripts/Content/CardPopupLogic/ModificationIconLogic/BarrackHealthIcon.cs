using Project.Architecture;
using Project.Content.BuildSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Content.UI
{
    public class BarrackHealthIcon : MonoBehaviour
    {
        [SerializeField] private BarracksType _type;

        [SerializeField] private TextMeshProUGUI _baseText;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private TextMeshProUGUI _currentText;

        private SceneData _sceneData;
        private BarrackDynamicData _dynamicData;
        private BarrackConfig _config;

        [Inject]
        private void Construct(SceneData sceneData)
        {
            _sceneData = sceneData;
            MainSceneBootstrap.OnServicesInitialized += Initialize;
        }

        private void Initialize()
        {
            _dynamicData = _sceneData.BarrackDynamicData[_type];
            _config = _dynamicData.Config;

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_dynamicData == null)
                return;

            _baseText.text = _config.BuildingMaxHealth.ToString();
            _bonusText.text = (_dynamicData.BuildingMaxHealth.Value - _config.BuildingMaxHealth).ToString();
            _currentText.text = _dynamicData.BuildingMaxHealth.Value.ToString();
        }

        private void OnDestroy()
        {
            MainSceneBootstrap.OnServicesInitialized -= Initialize;
        }
    }
}
