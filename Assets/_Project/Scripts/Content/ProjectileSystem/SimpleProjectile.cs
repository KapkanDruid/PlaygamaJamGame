using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Project.Content.ProjectileSystem
{
    public class SimpleProjectile : Projectile
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private EffectType _shootEffect;
        [SerializeField] private EffectType _hitEffect;
        [SerializeField] private ProjectileType _projectileType;

        private PauseHandler _pauseHandler;
        private AudioController _audioController;

        [SerializeField] private float _damage;
        private float _speed;
        private float _lifeTime;
        private float _elapsedTime;
        private bool _isActive;

        private Vector2 _moveDirection;

        public ProjectileType ProjectileType => _projectileType;

        [Inject]
        private void Construct(PauseHandler pauseHandler, AudioController audioController)
        {
            _pauseHandler = pauseHandler;
            _audioController = audioController;
        }

        public void Prepare(Vector2 startPosition, Vector2 moveDirection, IProjectileData projectileData)
        {
            PrepareProjectile(projectileData.EnemyFlag);
            _damage = projectileData.Damage;
            _speed = projectileData.Speed;
            _lifeTime = projectileData.LifeTime;

            _moveDirection = moveDirection;
            transform.position = startPosition;

            transform.right = moveDirection;

            _elapsedTime = 0;

            if (_animator != null)
            {
                _animator.SetTrigger(AnimatorHashes.ShootTrigger);
                WaitForAnimation(() => _isActive = true).Forget();
            }
            else
                _isActive = true;

            if (_shootEffect != null)
                _audioController.PlayOneShot(_shootEffect);
        }

        private void FixedUpdate()
        {
            if (_pauseHandler.IsPaused)
                return;

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

            Vector2 nextPosition = Rigidbody.position + (_moveDirection.normalized * _speed * Time.deltaTime);

            Rigidbody.MovePosition(nextPosition);
        }

        protected override void OnTargetCollision(Collider2D collision, IEntity entity)
        {
            var damageable = entity.ProvideComponent<IDamageable>();
            damageable.TakeDamage(_damage);

            if (_hitEffect != null)
                _audioController.PlayOneShot(_hitEffect);

            if (_animator != null)
            {
                _animator.SetTrigger(AnimatorHashes.HitTrigger);
                _isActive = false;
                WaitForAnimation(() => gameObject.SetActive(false)).Forget();
            }
            else
                gameObject.SetActive(false);
        }

        private async UniTask WaitForAnimation(Action action)
        {
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

            action?.Invoke();
        }
    }
}