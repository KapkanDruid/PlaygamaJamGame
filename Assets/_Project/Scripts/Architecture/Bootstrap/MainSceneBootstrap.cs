using Project.Content;
using Project.Content.BuildSystem;
using Project.Content.CharacterAI;
using Project.Content.UI;
using System;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainSceneBootstrap : MonoBehaviour
    {
        [Inject] private InputSystemActions _inputActions;
        [Inject] private SceneData _sceneData;
        [Inject] private CardsPopupView _cardsPopupView;
        [Inject] private CardsPopupPresenter _cardsPopupPresenter;
        [Inject] private LevelExperienceController _levelExperienceHandler;
        [Inject] private FloatingTextHandler _floatingTextHandler;
        [Inject] private UpgradeEffectController _upgradeEffectController;
        [Inject] private AudioController _audioController;
        [Inject] private EntityCommander _entityCommander;

        public static event Action OnServicesInitialized;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _sceneData.Initialize();
            _inputActions.Enable();
            _cardsPopupView.Initialize();
            _cardsPopupPresenter.Initialize();
            _levelExperienceHandler.Initialize();
            _floatingTextHandler.Initialize();
            _upgradeEffectController.Initialize();
            _audioController.Initialize();
            _entityCommander.Initialize();

            OnServicesInitialized?.Invoke();
        }

        private void Dispose()
        {
            _inputActions.Disable();
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}