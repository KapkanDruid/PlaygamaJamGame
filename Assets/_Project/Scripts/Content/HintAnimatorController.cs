using UnityEngine;

namespace Project.Content.Spawners
{
    public class HintAnimatorController : MonoBehaviour
    {
        private Animator _animator;
        private float _animationSpeed;
        private float _animationTime;
        private bool _isPlaying;

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


    }
}
