using Project.Content.UI.DataModification;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.UI
{
    public class DataModifierCard : CoreProgressCard
    {
        [SerializeField] private DataModifierConfig _modifierConfig;
        [SerializeField] private Button _button;

        public override Button Button => _button;
        public override event Action<ICoreProgressStrategy> OnCardSelected;

        [Inject]
        private void Construct(SceneData sceneData)
        {
            _modifierConfig.SetSceneData(sceneData);
        }

        private void Start()
        {
            _button.onClick.AddListener(() => OnCardSelected?.Invoke(_modifierConfig));
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
