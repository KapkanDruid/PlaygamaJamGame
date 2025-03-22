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

        public EnemyDeadHandler(Transform transform)
        {
            _characterTransform = transform;
            _isDead = false;
        }

        public EnemyDeadHandler(Animator animator)
        {
            _animator = animator;
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

            //DestroyThisAsync().Forget();
        }

        private async UniTask DestroyThisAsync()
        {

            await WaitForAnimationState();

            GameObject gameObject = _characterTransform.gameObject;

            if (gameObject == null)
            {
                Debug.LogError("gameObject is null!");
                return;
            }

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
