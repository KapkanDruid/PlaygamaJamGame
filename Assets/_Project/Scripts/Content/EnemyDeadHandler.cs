using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Project.Content
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
            if (_animator == null)
                Debug.Log("_animator = null");
            if (_animator != null)
                _animator.SetBool(AnimatorHashes.IsDead, true);
            _isDead = true;
            OnDeath?.Invoke();

            await WaitForAnimationState();

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

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            try
            {
                await UniTask.WaitForSeconds(stateInfo.length, cancellationToken: _characterTransform.gameObject.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}
