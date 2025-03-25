using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class DirectProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private Animator _animator;

        private EntityFlags _enemyFlag;
        private float _damage;
        private float _speed;
        private float _lifeTime;
        private float _elapsedTime;
        private bool _isActive;

        private Vector2 _moveDirection;

        public void Prepare(Vector2 startPosition, Vector2 moveDirection, IDirectProjectileData directProjectileData)
        {
            _enemyFlag = directProjectileData.EnemyFlag;
            _damage = directProjectileData.Damage;
            _speed = directProjectileData.Speed;
            _lifeTime = directProjectileData.LifeTime;

            _moveDirection = moveDirection;
            transform.position = startPosition;

            transform.right = moveDirection;
            _elapsedTime = 0;
            _isActive = true;
            if (_animator != null)
                _animator.SetTrigger(AnimatorHashes.ShootTrigger);
        }
         
        private void FixedUpdate()
        {
            Move();

            if (_elapsedTime > _lifeTime)
            {
                gameObject.SetActive(false);
            }
        }

        private void Move()
        {
            if (!_isActive)
                return;

            _elapsedTime += Time.deltaTime;

            Vector2 nextPosition = _rigidBody.position + (_moveDirection.normalized * _speed * Time.deltaTime);

            _rigidBody.MovePosition(nextPosition);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isActive)
                return;

            if (collision.gameObject.TryGetComponent(out IEntity entity))
            {
                Flags flags = entity.ProvideComponent<Flags>();

                if (flags == null)
                    return;

                if (!flags.Contain(_enemyFlag))
                    return;

                IDamageable damageable = entity.ProvideComponent<IDamageable>();

                if (damageable == null)
                    return;

                bool isDamageDone = false;

                damageable.TakeDamage(_damage, () => isDamageDone = true);

                if (!isDamageDone)
                    return;

                WaitForAnimation().Forget();
            }
        }

        private async UniTask WaitForAnimation()
        {
            _isActive = false;

            if (_animator != null)
            {
                _animator.SetTrigger(AnimatorHashes.HitTrigger);

                try
                {
                    await UniTask.WaitForFixedUpdate(this.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

                try
                {
                    await UniTask.WaitForSeconds(stateInfo.length, cancellationToken: this.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }

            gameObject.SetActive(false);
        }
    }
}