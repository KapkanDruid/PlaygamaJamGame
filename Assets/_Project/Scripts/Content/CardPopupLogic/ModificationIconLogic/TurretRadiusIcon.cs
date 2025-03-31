using Project.Architecture;
using Project.Content.BuildSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Content.UI
{
    public class TurretRadiusIcon : MonoBehaviour
    {
        [SerializeField] private TurretType _turretType;

        [SerializeField] private TextMeshProUGUI _baseText;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private TextMeshProUGUI _currentText;

        private SceneData _sceneData;
        private TurretDynamicData _dynamicData;
        private TurretConfig _config;

        [Inject]
        private void Construct(SceneData sceneData)
        {
            _sceneData = sceneData;
            MainSceneBootstrap.OnServicesInitialized += Initialize;
        }

        private void Initialize()
        {
            _dynamicData = _sceneData.TurretDynamicData[_turretType];
            _config = _dynamicData.Config;

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_dynamicData == null)
                return;

            _baseText.text = _config.SensorRadius.ToString();
            _bonusText.text = (_dynamicData.SensorRadius.Value - _config.SensorRadius).ToString();
            _currentText.text = _dynamicData.SensorRadius.Value.ToString();
        }

        private void OnDestroy()
        {
            MainSceneBootstrap.OnServicesInitialized -= Initialize;
        }
    }
}
