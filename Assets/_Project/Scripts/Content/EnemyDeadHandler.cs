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
                _animator.SetTrigger(AnimatorHashes.DeathTrigger);

            await WaitForAnimationState();

            _characterTransform.gameObject.SetActive(false);
            _isDead = true;

        }

        private async UniTask WaitForAnimationState()
        {
            if (_animator == null)
                return;

            try
            {
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
