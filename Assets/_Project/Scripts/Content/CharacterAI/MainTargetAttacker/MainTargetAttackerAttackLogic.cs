using Project.Content.BuildSystem;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    class MainTargetAttackerAttackLogic : ITickable, IGizmosDrawer
    {
        private readonly ICharacterData _characterData;
        private readonly IAttackerData _attackerData;
        private IDamageable _damageable;
        private MainTargetAttackerEntity _mainTargetAttackerEntity;
        private MainTargetAttackerData _mainTargetAttackerData;
        private Animator _animator;
        private PauseHandler _pauseHandler;
        private float _attackCooldownTimer;
        private EnemyDeadHandler _enemyDeadHandler;

        public MainTargetAttackerAttackLogic(MainTargetAttackerEntity mainTargetAttackerEntity,
                                             Animator animator,
                                             PauseHandler pauseHandler,
                                             EnemyDeadHandler enemyDeadHandler,
                                             ICharacterData characterData,
                                             IAttackerData attackerData,
                                             MainTargetAttackerData mainTargetAttackerData)
        {
            _characterData = characterData;
            _attackerData = attackerData;
            _mainTargetAttackerData = mainTargetAttackerData;
            _mainTargetAttackerEntity = mainTargetAttackerEntity;
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

            if (_mainTargetAttackerEntity.TargetTransform == null)
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

            _damageable = null;

            if (_mainTargetAttackerEntity.PathInvalid)
            {
                TryToHitEntity(_mainTargetAttackerEntity.BlockingEntity);
            }
            else
            {
                TryToHitEntity(_mainTargetAttackerEntity.TargetEntity);
            }

            Attack();
            _attackCooldownTimer = _characterData.AttackCooldown;
        }

        private void TryToHitEntity(IEntity entity)
        {
            if (entity == null)
                return;

            var targetCollider = entity.ProvideComponent<Collider2D>();
            if (targetCollider == null)
                return;

            var closestColliderPoint = targetCollider.ClosestPoint(_mainTargetAttackerEntity.transform.position);
            if (CheckDistanceToTarget(closestColliderPoint))
            {
                _damageable = entity.ProvideComponent<IDamageable>();
            }
        }

        private bool CheckDistanceToTarget(Vector2 closestPoint)
        {
            return Vector2.Distance(_mainTargetAttackerEntity.transform.position, closestPoint) <= _characterData.DistanceToTarget + _attackerData.HitColliderSize;
        }

        private void Attack()
        {
            if (_damageable == null)
                return;

            _animator.SetTrigger(AnimatorHashes.SpikeAttackTrigger);
            _damageable?.TakeDamage(_characterData.Damage);
        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)_mainTargetAttackerData.SensorData.SensorOrigin.position + _attackerData.HitColliderOffset, _attackerData.HitColliderSize);

            Gizmos.color = Color.green;
            Gizmos.DrawLine((Vector2)_mainTargetAttackerData.SensorData.SensorOrigin.position, (Vector2)_mainTargetAttackerEntity.TargetTransform.position);

#endif
        }

    }
}
