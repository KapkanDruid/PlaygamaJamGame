using Project.Content;
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