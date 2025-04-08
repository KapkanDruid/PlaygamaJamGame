using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Project.Content.CharacterAI
{
    public class EnemyDeadHandler
    {
        private bool _isDead;

        private Transform _characterTransform;
        private Animator _animator;

        public event Action OnDeath;
        public bool IsDead => _isDead;

        public EnemyDeadHandler(Transform transform, Animator animator)
        {
            _characterTransform = transform;
            _animator = animator;
            _isDead = false;
        }

        public void Reset()
        {
            _isDead = false;
            _characterTransform.gameObject.SetActive(true);
        }

        public async void Death()
        {
            if (_isDead)
                return;
            _isDead = true;
            if (_animator == null)
                Debug.Log("_animator = null");
            if (_animator != null)
                _animator.SetBool(AnimatorHashes.IsDead, _isDead);
            OnDeath?.Invoke();

            await WaitForAnimationState();

            if (_characterTransform == null)
                return;

            _characterTransform.gameObject.SetActive(false);
        }

        private async UniTask WaitForAnimationState()
        {
            if (_animator == null)
                return;

            try
            {
                await UniTask.WaitForFixedUpdate(_characterTransform.gameObject.GetCancellationTokenOnDestroy());
                await UniTask.WaitForFixedUpdate(_characterTransform.gameObject.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException)
            {
                return;
            }

            try
            {
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                while (stateInfo.normalizedTime < 1)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _characterTransform.gameObject.GetCancellationTokenOnDestroy());
                    stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}
