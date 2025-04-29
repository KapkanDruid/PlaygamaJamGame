using Project.Content.ObjectPool;
using Project.Content.ProjectileSystem;
using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Infantryman
{
    public class InfantrymanAttackLogic : ITickable
    {
        private readonly IProjectileTypeData _poolData;
        private readonly IShooterData _shootData;
        private readonly Transform _shootPoint;
        private readonly FiltrablePoolsHandler _poolsHandler;

        private IAllyEntityData _infantrymanData;
        private InfantrymanEntity _infantrymanEntity;
        private float _attackCooldownTimer;
        private Animator _animator;
        private PauseHandler _pauseHandler;
        private EnemyDeadHandler _enemyDeadHandler;

        private Predicate<SimpleProjectile> ProjectilePredicate
            => item => item.ProjectileType == _poolData.ProjectileType && item.gameObject.activeInHierarchy == false;

        public InfantrymanAttackLogic(InfantrymanEntity infantrymanEntity,
                                      IProjectileTypeData projectilePoolData,
                                      IShooterData shootData,
                                      Animator animator,
                                      PauseHandler pauseHandler,
                                      EnemyDeadHandler enemyDeadHandler,
                                      FiltrablePoolsHandler poolsHandler = null)
        {
            _infantrymanEntity = infantrymanEntity;
            _infantrymanData = infantrymanEntity.InfantrymanData;
            _poolData = projectilePoolData;
            _shootData = shootData;
            _shootPoint = _shootData.ShootPoint;
            _animator = animator;
            _pauseHandler = pauseHandler;
            _enemyDeadHandler = enemyDeadHandler;
            _poolsHandler = poolsHandler;
        }

        public void Tick()
        {
            if (_pauseHandler.IsPaused)
                return;

            if (_enemyDeadHandler.IsDead)
                return;

            TryToShoot();

            CooldownAttack();
        }

        private void TryToShoot()
        {
            if (_infantrymanEntity.TargetTransform == null)
                return;

            if (Vector3.Distance(_infantrymanEntity.transform.position, _infantrymanEntity.TargetTransform.position) <= _infantrymanData.AttackRange)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (_attackCooldownTimer <= 0)
            {
                _animator.SetTrigger(AnimatorHashes.RangeAttackTrigger);

                var projectile = _poolsHandler.GetByPredicate<SimpleProjectile>(ProjectilePredicate);

                Vector2 direction = _infantrymanEntity.TargetTransform.position - _shootPoint.position;

                projectile.Prepare(_shootPoint.position, direction, _shootData.ProjectileData);

                _attackCooldownTimer = _infantrymanData.AttackCooldown;
            }
        }

        private void CooldownAttack()
        {
            if (_attackCooldownTimer > 0)
            {
                _attackCooldownTimer -= Time.deltaTime;
            }
        }
    }
}

