using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class WinView : MonoBehaviour
    {
        [Header("Win Menu")]
        [SerializeField] private float _timeAnimationButtons = 0.5f;
        [SerializeField] private float _timeAnimationBackground = 0.5f;
        [SerializeField] private float _delayToAnimation;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private GameObject _panel;
        [SerializeField] private RectTransform _background;
        [SerializeField] private EffectType _music;

        private WinLoseHandler _winLoseHandler;
        private AudioController _audioController;
        private Button[] _buttons;
        private bool _isShowing;

        [Inject]
        public void Construct(WinLoseHandler winLoseHandler, AudioController audioController)
        {
            _winLoseHandler = winLoseHandler;
            _audioController = audioController;
        }

        private void Start()
        {
            _winLoseHandler.OnWin += ShowVictoryMenu;

            _background.gameObject.SetActive(false);
            _buttonsContainer.SetActive(false);
            _panel.SetActive(false);

            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();
            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }
        }

        private void ShowVictoryMenu()
        {
            _background.gameObject.SetActive(true);
            _animator.SetTrigger(AnimatorHashes.EndSceneTrigger);

            _audioController.StopMusic();
            _audioController.PLayLoopEffect(_music);

            _panel.SetActive(true);

            ShowAsync().Forget();
        }

        private async UniTask ShowAsync()
        {
            if (_isShowing)
                return;

            _isShowing = true;

            try
            {
                await UniTask.WaitForSeconds(_delayToAnimation);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            ShowButtons();

            _isShowing = false;
        }

        private void ShowButtons()
        {
            _buttonsContainer.SetActive(true);

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].transform.DOScale(1f, _timeAnimationButtons)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(i * 0.2f);
            }
        }

        private void OnDestroy()
        {
            _winLoseHandler.OnWin -= ShowVictoryMenu;
        }
    }
}
