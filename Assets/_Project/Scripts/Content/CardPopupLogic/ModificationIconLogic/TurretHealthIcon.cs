using Project.Architecture;
using Project.Content.BuildSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Content.UI
{
    public class TurretHealthIcon : MonoBehaviour
    {
        [SerializeField] private TurretType _turretType;

        [SerializeField] private TextMeshProUGUI _baseText;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private TextMeshProUGUI _currentText;

        private SceneData _sceneData;
        private TurretDynamicData _dynamicData;
        private TurretConfig _config;
        private bool _isInited;

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
        }

        private void OnEnable()
        {

            if (_dynamicData == null)
                return;

            _baseText.text = _config.MaxHealth.ToString();
            _bonusText.text = (_dynamicData.MaxHealth.Value - _config.MaxHealth).ToString();
            _currentText.text = _dynamicData.MaxHealth.Value.ToString();
        }

        private void OnDestroy()
        {
            MainSceneBootstrap.OnServicesInitialized -= Initialize;
        }
    }
}
