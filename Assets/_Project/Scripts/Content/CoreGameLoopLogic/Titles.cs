using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class Titles : MonoBehaviour, ISkipHandlerData
    {
        [SerializeField] private MusicType _musicType;
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _skipFiller;
        [SerializeField] private Image _background;
        [SerializeField] private float _fadeTime;
        [SerializeField] private float _skipDuration;
        [SerializeField] private UnityEvent _onAnimationEnd;

        private AudioController _audioController;
        private SkipHandler _skipHandler;

        public Image SkipFiller => _skipFiller;
        public float SkipDuration => _skipDuration;

        [Inject]
        private void Construct(AudioController audioController, SkipHandler skipHandler)
        {
            _audioController = audioController;
            _skipHandler = skipHandler;
        }

        private void Start()
        {
            _skipHandler.Initialize(this, this.GetCancellationTokenOnDestroy());
        }

        public void ShowTitles()
        {
            _animator.SetTrigger(AnimatorHashes.ShowTitlesTrigger);
            _audioController.PlayMusic(_musicType);

            _skipHandler.IsActive = true;

            HandleAnimationEndAsync().Forget();
        }

        public void HideTitles()
        {
            _background.DOFade(0, _fadeTime);
            _skipHandler.IsActive = false;
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
                    if (_skipHandler.IsForceSkip)
                    {
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
    }
}
