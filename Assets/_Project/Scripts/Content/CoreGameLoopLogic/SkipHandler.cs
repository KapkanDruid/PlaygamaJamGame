using Cysharp.Threading.Tasks;
using Project.Architecture;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Project.Content.CoreGameLoopLogic
{
    public class SkipHandler : IDisposable
    {
        private readonly InputSystemActions _inputActions;
        private CancellationToken _cancellationToken;

        private Image _skipFiller;
        private float _skipDuration;

        private bool _isForceSkip;
        private bool _isActive;
        private bool _isSkipPressed;
        private float _currentSkipTime;

        public bool IsForceSkip => _isForceSkip;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (!value)
                {
                    _isForceSkip = false;
                }

                _isActive = value;
            }
        }

        public SkipHandler(InputSystemActions inputActions)
        {
            _inputActions = inputActions;

            _inputActions.UI.Skip.performed += OnSkipPressed;
            _inputActions.UI.Skip.canceled += OnSkipReleased;
        }

        public void Initialize(ISkipHandlerData data, CancellationToken cancellationToken)
        {
            _skipDuration = data.SkipDuration;
            _skipFiller = data.SkipFiller;
            _cancellationToken = cancellationToken;
            _isSkipPressed = false;
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
                    await UniTask.Yield(PlayerLoopTiming.Update, _cancellationToken);
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
                    await UniTask.Yield(PlayerLoopTiming.Update, _cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        public void Dispose()
        {
            if (_inputActions == null)
                return;

            _inputActions.UI.Skip.performed -= OnSkipPressed;
            _inputActions.UI.Skip.canceled -= OnSkipReleased;
        }
    }
}
