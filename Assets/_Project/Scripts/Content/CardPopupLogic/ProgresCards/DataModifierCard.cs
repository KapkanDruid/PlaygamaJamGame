using Project.Architecture;
using Project.Content.UI.DataModification;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.UI
{
    public class DataModifierCard : CoreProgressCard
    {
        [SerializeField] private DataModifierConfig _modifierConfig;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _modifierText;

        public override Button Button => _button;
        public override event Action<ICoreProgressStrategy> OnCardSelected;
        public static event Action<DataModifierConfig> OnModifierApplied;

        [Inject]
        private void Construct(SceneData sceneData)
        {
            _modifierConfig.SetSceneData(sceneData);

            MainSceneBootstrap.OnServicesInitialized += Initialize;
        }

        private void Initialize()
        {
            _button.onClick.AddListener(() =>
            {
                OnModifierApplied?.Invoke(_modifierConfig);
                OnCardSelected?.Invoke(_modifierConfig);
            });

            if (_modifierText == null)
                return;

            _modifierText.text = _modifierConfig.GetModifierValue().ToString();
        }

        private void OnEnable()
        {
            if (_modifierText == null)
                return;

            _modifierText.text = _modifierConfig.GetModifierValue().ToString();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            MainSceneBootstrap.OnServicesInitialized -= Initialize;
        }
    }
}
