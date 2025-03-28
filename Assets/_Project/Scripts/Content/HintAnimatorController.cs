using UnityEngine;
using Zenject;

namespace Project.Content.Spawners
{
    public class HintAnimatorController : MonoBehaviour
    {
        [SerializeField] private EffectType _effectType;

        private AudioController _controller;

        private Animator _animator;
        private float _animationSpeed;
        private float _animationTime;
        private bool _isPlaying;
        private AnimatorStateInfo _pausedAnimatorState;
        private PauseHandler _pauseHandler;

        [Inject]
        private void Construct(AudioController audioController, PauseHandler pauseHandler)
        {
            _controller = audioController;
            _pauseHandler = pauseHandler;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
            _animator = GetComponent<Animator>();
        }

        public void PlayAnimation(int duration, float speed)
        {
            if (_animator == null)
                return;

            gameObject.SetActive(true);
            _animationSpeed = speed;
            _animator.speed = _animationSpeed;
            _animationTime = duration;
            _isPlaying = true;
        }

        private void Update()
        {
            if (_pauseHandler.IsPaused)
            {
                PauseAnimation();
                return;
            }
            else
            {
                ResumeAnimation();
            }

            if (_isPlaying)
            {
                if (_animationTime > 0)
                {
                    _animator.SetTrigger(AnimatorHashes.HintTrigger);
                    _animationTime -= Time.deltaTime;
                }
                else
                {
                    gameObject.SetActive(false);
                    _isPlaying = false;
                }
            }
        }

        private void PauseAnimation()
        {
            if (_animator.speed != 0)
            {
                _pausedAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
                _animator.speed = 0;
            }
        }

        private void ResumeAnimation()
        {
            if (_animator.speed == 0)
            {
                _animator.speed = 1;
                _animator.Play(_pausedAnimatorState.fullPathHash, -1, _pausedAnimatorState.normalizedTime);
            }
        }

        private void OnHit()
        {
            _controller.PlayOneShot(_effectType);
        }
    }
}
