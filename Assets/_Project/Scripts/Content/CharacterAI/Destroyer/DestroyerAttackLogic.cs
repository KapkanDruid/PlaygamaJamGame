using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerAttackLogic : ITickable, IGizmosDrawer
    {
        private ICharacterData _destroyerData;
        private ISensorData _destroyerSensorData;
        private IDamageable _damageable;
        private CharacterSensor _characterSensor;
        private DestroyerHandler _destroyerHandler;
        private Animator _animator;
        private float _attackCooldownTimer;
        private PauseHandler _pauseHandler;
        private AnimatorStateInfo _pausedAnimatorState;

        public DestroyerAttackLogic(DestroyerHandler destroyerHandler,
                                    CharacterSensor characterSensor,
                                    Animator animator,
                                    PauseHandler pauseHandler)
        {
            _destroyerHandler = destroyerHandler;
            _destroyerData = destroyerHandler.DestroyerData;
            _destroyerSensorData = (ISensorData)destroyerHandler.DestroyerData;
            _characterSensor = characterSensor;
            _animator = animator;
            _pauseHandler = pauseHandler;
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

            if (_characterSensor.TargetToAttack == null || _characterSensor.TargetTransformToAttack == null)
            {
                _characterSensor.ScanAreaToAttack();

                if (_characterSensor.TargetTransformToAttack != null)
                {
                    if (!_characterSensor.TargetTransformToAttack.gameObject.activeInHierarchy)
                        _characterSensor.ScanAreaToAttack();
                }
            }

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
            if (_characterSensor.TargetToAttack != null && _characterSensor.TargetTransformToAttack != null)
            {
                if (Vector2.Distance(_destroyerHandler.transform.position, _characterSensor.TargetTransformToAttack.position) <= _destroyerHandler.DestroyerData.DistanceToTarget + _destroyerSensorData.HitColliderSize * 2)
                {
                    if (_attackCooldownTimer <= 0)
                    {
                        _animator.SetTrigger(AnimatorHashes.SpikeAttackTrigger);
                        _damageable = _characterSensor.TargetToAttack.ProvideComponent<IDamageable>();
                        Attack();
                        _attackCooldownTimer = _destroyerData.AttackCooldown;
                    }
                }
            }
        }

        private void Attack()
        {
            if (_damageable == null)
                return;

            _damageable?.TakeDamage(_destroyerData.Damage);
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

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)_destroyerSensorData.CharacterTransform.position + _destroyerSensorData.HitColliderOffset, _destroyerSensorData.HitColliderSize);
#endif
        }

    }
}

