using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerAttackLogic : IDisposable, ITickable, IGizmosDrawer
    {
        private ICharacterData _destroyerData;
        private ISensorData _destroyerSensorData;
        private IDamageable _damageable;
        private CharacterSensor _characterSensor;
        private DestroyerHandler _destroyerHandler;
        private AnimatorEventHandler _animatorEventHandler;
        private GizmosDrawer _gizmosDrawer;
        private Animator _animator;
        private float _attackCooldownTimer;

        public DestroyerAttackLogic(DestroyerHandler destroyerHandler, CharacterSensor characterSensor, AnimatorEventHandler animatorEventHandler, Animator animator, GizmosDrawer gizmosDrawer)
        {
            _destroyerHandler = destroyerHandler;
            _destroyerData = destroyerHandler.DestroyerData;
            _destroyerSensorData = (ISensorData)destroyerHandler.DestroyerData;
            _characterSensor = characterSensor;
            _animatorEventHandler = animatorEventHandler;
            _animator = animator;
            _gizmosDrawer = gizmosDrawer;

            _gizmosDrawer.AddGizmosDrawer(this);
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
                    _attackCooldownTimer = _destroyerData.AttackCooldown;
                }
            }
        }

        private void Attack()
        {
            _damageable = null;
            CheckAreaToAttack();

            if (_damageable == null)
                return;

            if (_destroyerHandler.CanAttack)
            {
                _damageable?.TakeDamage(_destroyerData.Damage);
            }

        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)_destroyerSensorData.CharacterTransform.position + _destroyerSensorData.HitColliderOffset, _destroyerSensorData.HitColliderSize);
        }

        private void CheckAreaToAttack()
        {
            Vector2 origin = (Vector2)_destroyerSensorData.CharacterTransform.position + _destroyerSensorData.HitColliderOffset;
            Vector2 direction = Vector2.right;
            float size = _destroyerSensorData.HitColliderSize;

            var _hits = Physics2D.CircleCastAll(origin, size, direction, 0f);

            int count = _hits.Length;
            for (int i = 0; i < count; i++)
            {
                if (!_hits[i].collider.TryGetComponent(out IEntity entity))
                    continue;

                if (entity == _destroyerSensorData.ThisEntity)
                    continue;

                Flags flags = entity.ProvideComponent<Flags>();

                if (flags == null)
                    continue;

                foreach (var flag in _destroyerSensorData.EnemyFlag)
                {
                    if (!flags.Contain(flag))
                        continue;
                }

                _damageable = entity.ProvideComponent<IDamageable>();
            }
        }
    }
}

