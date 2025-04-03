using Cysharp.Threading.Tasks;
using Project.Content.BuildSystem;
using System;
using UnityEngine;
using Zenject;

namespace Project.Content.ProjectileSystem
{
    public class SimpleProjectile : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Animator _animator;
        [SerializeField] private EffectType _shootEffect;
        [SerializeField] private EffectType _hitEffect;

        private PauseHandler _pauseHandler;
        private AudioController _audioController;

        private EntityFlags[] _enemyFlag;
        private float _damage;
        private float _speed;
        private float _lifeTime;
        private float _elapsedTime;
        private bool _isActive;

        private Vector2 _moveDirection;


        [Inject]
        private void Construct(PauseHandler pauseHandler, AudioController audioController)
        {
            _pauseHandler = pauseHandler;
            _audioController = audioController;
        }

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

            Vector2 nextPosition = _projectilePrefab.Rigidbody.position + (_moveDirection.normalized * _speed * Time.deltaTime);

            _projectilePrefab.Rigidbody.MovePosition(nextPosition);
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