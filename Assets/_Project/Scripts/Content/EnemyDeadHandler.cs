using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Content
{
    public class EnemyDeadHandler
    {
        private bool _isDead;

        private Transform _characterTransform;
        private Animator _animator;

        public bool IsDead => _isDead;

        public EnemyDeadHandler(Animator animator)
        {
            _animator = animator;
        }

        public void Initialize(Transform characterTransform)
        {
            _characterTransform = characterTransform;
            _isDead = false;
        }

        public void Death()
        {
            DestroyThisAsync().Forget();
            _isDead = true;
        }

        private async UniTask DestroyThisAsync()
        {
            if (_isDead)
                return;
            if (_animator == null)
                Debug.Log("_animator = null");
            if (_animator != null)
                _animator.SetTrigger(AnimatorHashes.DeathTrigger);

            await WaitForAnimationState();

            GameObject gameObject = _characterTransform.gameObject;

            if (gameObject == null)
            {
                Debug.LogError("gameObject is null!");
                return;
            }

            UnityEngine.Object.Destroy(gameObject);
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
