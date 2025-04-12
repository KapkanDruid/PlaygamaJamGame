using Project.Content.BuildSystem;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerAttackLogic : ITickable
    {
        private readonly ICharacterData _characterData;
        private readonly IAttackerData _attackerData;
        private IDamageable _damageable;
        private DestroyerEntity _destroyerEntity;
        private Animator _animator;
        private float _attackCooldownTimer;
        private PauseHandler _pauseHandler;
        private EnemyDeadHandler _enemyDeadHandler;

        public DestroyerAttackLogic(DestroyerEntity destroyerEntity,
                                    Animator animator,
                                    PauseHandler pauseHandler,
                                    EnemyDeadHandler enemyDeadHandler,
                                    ICharacterData characterData,
                                    IAttackerData attackerData)
        {
            _characterData = characterData;
            _attackerData = attackerData;
            _destroyerEntity = destroyerEntity;
            _animator = animator;
            _pauseHandler = pauseHandler;
            _enemyDeadHandler = enemyDeadHandler;
        }

        public void Tick()
        {
            if (_pauseHandler.IsPaused)
                return;

            if (_enemyDeadHandler.IsDead)
                return;

            if (_destroyerEntity.TargetTransform == null)
                return;

            TryToHit();

            CooldownAttack();
        }

        private void CooldownAttack()
        {
            if (_attackCooldownTimer > 0)
            {
                _attackCooldownTimer -= Time.deltaTime;
            }
        }

        private void TryToHit()
        {
            if (_attackCooldownTimer > 0)
                return;

            Collider2D targetCollider = _destroyerEntity.TargetEntity.ProvideComponent<Collider2D>();
            if (targetCollider == null)
                return;

            Vector3 closestColliderPoint = targetCollider.ClosestPoint(_destroyerEntity.transform.position);
            if (CheckDistanceToTarget(closestColliderPoint))
            {
                _damageable = _destroyerEntity.TargetEntity.ProvideComponent<IDamageable>();
                Attack();
                _attackCooldownTimer = _characterData.AttackCooldown;
            }
        }

        private bool CheckDistanceToTarget(Vector3 closestPoint)
        {
            return Vector2.Distance(_destroyerEntity.transform.position, closestPoint) <= _characterData.DistanceToTarget + _attackerData.HitColliderSize;
        }

        private void Attack()
        {
            if (_damageable == null)
                return;

            _animator.SetTrigger(AnimatorHashes.SpikeAttackTrigger);

            _damageable?.TakeDamage(_characterData.Damage);
        }

    }
}

