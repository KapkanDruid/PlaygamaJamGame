using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    class MainTargetAttackerAttackLogic : IDisposable, ITickable, IGizmosDrawer
    {
        private ICharacterData _mainTargetAttackerData;
        private ISensorData _mainTargetAttackerSensorData;
        private IDamageable _damageable;
        private CharacterSensor _characterSensor;
        private MainTargetAttackerHandler _mainTargetAttackerHandler;
        private AnimatorEventHandler _animatorEventHandler;
        private Animator _animator;
        private float _attackCooldownTimer;

        public MainTargetAttackerAttackLogic(MainTargetAttackerHandler mainTargetAttackerHandler,
                                             CharacterSensor characterSensor,
                                             AnimatorEventHandler animatorEventHandler,
                                             Animator animator)
        {
            _mainTargetAttackerHandler = mainTargetAttackerHandler;
            _mainTargetAttackerData = mainTargetAttackerHandler.MainTargetAttackerData;
            _mainTargetAttackerSensorData = (ISensorData)mainTargetAttackerHandler.MainTargetAttackerData;
            _characterSensor = characterSensor;
            _animatorEventHandler = animatorEventHandler;
            _animator = animator;
            _characterSensor.HasTargetToAttack += InvokeAttack;
            _animatorEventHandler.OnAnimationHit += TryToHit;
        }

        public void Dispose()
        {
            _animatorEventHandler.OnAnimationHit -= TryToHit;
            _characterSensor.HasTargetToAttack -= InvokeAttack;
        }

        public void Tick()
        {
            CooldownAttack();
            TryToHit();
        }

        private void CooldownAttack()
        {
            if (_attackCooldownTimer > 0)
            {
                _attackCooldownTimer -= Time.deltaTime;
            }
        }

        private void InvokeAttack()
        {
            _animator.SetTrigger(AnimatorHashes.SpikeAttackTrigger);
        }

        private void TryToHit()
        {
            if (_characterSensor.TargetToAttack != null && _characterSensor.TargetTransformToAttack != null)
            {
                if (_attackCooldownTimer <= 0)
                {
                    Attack();
                    _attackCooldownTimer = _mainTargetAttackerData.AttackCooldown;
                }
            }
        }

        private void Attack()
        {
            _damageable = null;
            CheckAreaToAttack();

            if (_damageable == null)
                return;

            if (_mainTargetAttackerHandler.CanAttack)
            {
                _damageable?.TakeDamage(_mainTargetAttackerData.Damage);
            }

        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)_mainTargetAttackerSensorData.CharacterTransform.position + _mainTargetAttackerSensorData.HitColliderOffset, _mainTargetAttackerSensorData.HitColliderSize);
#endif
        }
        
        private void CheckAreaToAttack()
        {
            Vector2 origin = (Vector2)_mainTargetAttackerSensorData.CharacterTransform.position + _mainTargetAttackerSensorData.HitColliderOffset;
            Vector2 direction = Vector2.right;
            float size = _mainTargetAttackerSensorData.HitColliderSize;

            var _hits = Physics2D.CircleCastAll(origin, size, direction, 0f);

            int count = _hits.Length;
            for (int i = 0; i < count; i++)
            {
                if (!_hits[i].collider.TryGetComponent(out IEntity entity))
                    continue;

                if (entity == _mainTargetAttackerSensorData.ThisEntity)
                    continue;

                Flags flags = entity.ProvideComponent<Flags>();

                if (flags == null)
                    continue;

                if (_mainTargetAttackerHandler.PathInvalid)
                {
                    if (!flags.Contain(EntityFlags.Player))
                        continue;
                }
                else
                {
                    foreach (var flag in _mainTargetAttackerSensorData.EnemyFlag)
                    {
                        if (!flags.Contain(flag))
                            continue;
                    }
                }

                _damageable = entity.ProvideComponent<IDamageable>();
            }
        }
    }
}
