using Project.Architecture;
using Project.Content.BuildSystem;
using Project.Content.CharacterAI.Infantryman;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Content.UI
{
    public class InfantryDamageIcon : MonoBehaviour
    {
        [SerializeField] private BarracksType _type;
        [SerializeField] private InfantrymanConfig _config;


        [SerializeField] private TextMeshProUGUI _baseText;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private TextMeshProUGUI _currentText;

        private SceneData _sceneData;
        private BarrackDynamicData _dynamicData;

        [Inject]
        private void Construct(SceneData sceneData)
        {
            _sceneData = sceneData;
            MainSceneBootstrap.OnServicesInitialized += Initialize;
        }

        private void Initialize()
        {
            _dynamicData = _sceneData.BarrackDynamicData[_type];

            _baseText.text = _config.Damage.ToString();
            _bonusText.text = _dynamicData.UnitDamageModifier.Value.ToString();
            _currentText.text = (_dynamicData.UnitDamageModifier.Value + _config.Damage).ToString();
        }

        private void OnEnable()
        {
            if (_dynamicData == null)
                return;

            _baseText.text = _config.Damage.ToString();
            _bonusText.text = _dynamicData.UnitDamageModifier.Value.ToString();
            _currentText.text = (_dynamicData.UnitDamageModifier.Value + _config.Damage).ToString();
        }

        private void OnDestroy()
        {
            MainSceneBootstrap.OnServicesInitialized -= Initialize;
        }
    }
}
