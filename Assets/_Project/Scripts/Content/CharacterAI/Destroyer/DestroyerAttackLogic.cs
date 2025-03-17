using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerAttackLogic : IDisposable, ITickable
    {
        private ICharacterData _destroyerData;
        private ISensorData _destroyerSensorData;
        private IDamageable _damageable;
        private CharacterSensor _characterSensor;
        private DestroyerHandler _destroyerHandler;


        public DestroyerAttackLogic(DestroyerHandler characterHandler, CharacterSensor characterSensor)
        {
            _destroyerHandler = characterHandler;
            _destroyerData = characterHandler.DestroyerData;
            _destroyerSensorData = (ISensorData)characterHandler.DestroyerData;
            _characterSensor = characterSensor;

            _characterSensor.HasTargetToAttack += Attack;
        }

        public void Tick()
        {
            TryToAttack();
        }

        public void Dispose()
        {
            _characterSensor.HasTargetToAttack -= Attack;
        }

        private void TryToAttack()
        {
            if (_characterSensor.TargetToAttack != null && _characterSensor.TargetTransformToAttack != null)
            {
                Attack();
            }
        }

        private void Attack()
        {
            CheckAreaToAttack();

            if (_destroyerHandler.CanAttack)
            {
                _damageable?.TakeDamage(_destroyerData.Damage);
            }
        }

        private void CheckAreaToAttack()
        {
            Vector2 origin = (Vector2)_destroyerSensorData.CharacterTransform.position + _destroyerSensorData.HitColliderOffset;
            Vector2 direction = Vector2.zero;
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

                if (!flags.Contain(_destroyerSensorData.EnemyFlag))
                    continue;

                _damageable = entity.ProvideComponent<IDamageable>();
            }
        }
    }
}

