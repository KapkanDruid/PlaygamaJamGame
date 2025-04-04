﻿using Project.Content.BuildSystem;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Infantryman
{
    public class InfantrymanAttackLogic : ITickable, IInitializable
    {
        private readonly IProjectilePoolData _poolData;
        private readonly IShooterData _shootData;
        private readonly Transform _shootPoint;

        private IAllyEntityData _infantrymanData;
        private InfantrymanEntity _infantrymanEntity;
        private float _attackCooldownTimer;
        private ObjectPooler<DirectProjectile> _projectilePool;
        private Animator _animator;
        private PauseHandler _pauseHandler;
        private DiContainer _diContainer;
        private AnimatorStateInfo _pausedAnimatorState;

        public InfantrymanAttackLogic(InfantrymanEntity infantrymanEntity,
                                      IProjectilePoolData projectilePoolData,
                                      IShooterData shootData,
                                      Animator animator,
                                      PauseHandler pauseHandler,
                                      DiContainer diContainer)
        {
            _infantrymanEntity = infantrymanEntity;
            _infantrymanData = infantrymanEntity.InfantrymanData;
            _poolData = projectilePoolData;
            _shootData = shootData;
            _shootPoint = _shootData.ShootPoint;
            _animator = animator;
            _pauseHandler = pauseHandler;
            _diContainer = diContainer;

            Initialize();
        }

        public void Initialize()
        {
            _projectilePool = new ObjectPooler<DirectProjectile>(_poolData.ProjectilePoolCount, "InfantrymanProjectiles", new InstantiateObjectContainer<DirectProjectile>(_poolData.ProjectilePrefab, _diContainer));
        }

        public void Tick()
        {
            if (_pauseHandler.IsPaused)
            {
                PauseAnimation();
                return;
            }
            else
            {
                ResumeAnimation();
            }

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
                var projectile = _projectilePool.Get();
                projectile.Prepare(_shootPoint.position, _infantrymanEntity.TargetTransform.position - _infantrymanEntity.transform.position, _shootData.ProjectileData);

                _attackCooldownTimer = _infantrymanData.AttackCooldown;
            }
        }

        private void PauseAnimation()
        {
            if (_animator.speed != 0)
            {
                _pausedAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
                _animator.speed = 0;
            }
        }

        private void ResumeAnimation()
        {
            if (_animator.speed == 0)
            {
                _animator.speed = 1;
                _animator.Play(_pausedAnimatorState.fullPathHash, -1, _pausedAnimatorState.normalizedTime);
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

