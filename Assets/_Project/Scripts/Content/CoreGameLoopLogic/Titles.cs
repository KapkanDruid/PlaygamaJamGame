using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Architecture;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class Titles : MonoBehaviour
    {
        [SerializeField] private MusicType _musicType;
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _skipFiller;
        [SerializeField] private Image _background;
        [SerializeField] private float _fadeTime;
        [SerializeField] private float _skipDuration;
        [SerializeField] private UnityEvent _onAnimationEnd;

        private AudioController _audioController;
        private InputSystemActions _inputActions;

        private bool _isSkipPressed;
        private bool _isForceSkip;
        private bool _isActive;
        private float _currentSkipTime;

        [Inject]
        private void Construct(AudioController audioController, InputSystemActions inputActions)
        {
            _audioController = audioController;
            _inputActions = inputActions;

            _inputActions.UI.Skip.performed += OnSkipPressed;
            _inputActions.UI.Skip.canceled += OnSkipReleased;
        }

        public void ShowTitles()
        {
            _animator.SetTrigger(AnimatorHashes.ShowTitlesTrigger);
            _audioController.PlayMusic(_musicType);

            _isActive = true;

            HandleAnimationEndAsync().Forget();
        }

        public void HideTitles()
        {
            _background.DOFade(0, _fadeTime);
            _isActive = false;
        }

        private void OnSkipPressed(InputAction.CallbackContext context)
        {
            Debug.Log("Pressed");
            if (!_isActive)
                return;

            _isSkipPressed = true;
            HandleSkipAsync().Forget();
        }

        private void OnSkipReleased(InputAction.CallbackContext context) => _isSkipPressed = false;

        private async UniTaskVoid HandleSkipAsync()
        {
            if (_isForceSkip)
                return;

            while (_isSkipPressed)
            {
                _currentSkipTime += Time.deltaTime;

                _skipFiller.fillAmount = _currentSkipTime / _skipDuration;

                if (_currentSkipTime >= _skipDuration)
                {
                    _currentSkipTime = _skipDuration;
                    _isForceSkip = true;
                    return;
                }

                try
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }


            while (!_isSkipPressed)
            {
                _currentSkipTime -= Time.deltaTime;

                _skipFiller.fillAmount = _currentSkipTime / _skipDuration;

                if (_currentSkipTime <= 0)
                {
                    _currentSkipTime = 0;
                    return;
                }

                try
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        private async UniTaskVoid HandleAnimationEndAsync()
        {
            try
            {
                if (_animator == null)
                    return;

                await UniTask.WaitForFixedUpdate(this.GetCancellationTokenOnDestroy());

                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                while (stateInfo.normalizedTime < 1)
                {
                    if (_isForceSkip)
                    {
                        _isForceSkip = false;
                        break;
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
                    stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                }

                _onAnimationEnd?.Invoke();
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        private void OnDestroy()
        {
            if (_inputActions == null)
                return;

            _inputActions.UI.Skip.performed -= OnSkipPressed;
            _inputActions.UI.Skip.canceled -= OnSkipReleased;
        }
    }
}
