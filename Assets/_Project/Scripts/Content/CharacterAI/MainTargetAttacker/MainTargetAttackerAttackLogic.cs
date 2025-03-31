using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    class MainTargetAttackerAttackLogic : ITickable, IGizmosDrawer
    {
        private ICharacterData _mainTargetAttackerData;
        private ISensorData _mainTargetAttackerSensorData;
        private IDamageable _damageable;
        private CharacterSensor _characterSensor;
        private MainTargetAttackerHandler _mainTargetAttackerHandler;
        private Animator _animator;
        private PauseHandler _pauseHandler;
        private float _attackCooldownTimer;
        private AnimatorStateInfo _pausedAnimatorState;

        public MainTargetAttackerAttackLogic(MainTargetAttackerHandler mainTargetAttackerHandler,
                                             CharacterSensor characterSensor,
                                             Animator animator,
                                             PauseHandler pauseHandler)
        {
            _mainTargetAttackerHandler = mainTargetAttackerHandler;
            _mainTargetAttackerData = mainTargetAttackerHandler.MainTargetAttackerData;
            _mainTargetAttackerSensorData = (ISensorData)mainTargetAttackerHandler.MainTargetAttackerData;
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
            if (_characterSensor.TargetToAttack != null)
            {
                if (_attackCooldownTimer <= 0)
                {
                    _damageable = null;
                    if (_mainTargetAttackerHandler.PathInvalid)
                    {
                        if (_mainTargetAttackerHandler.BlockingEntity == null)
                            return;

                        Collider2D targetCollider = _mainTargetAttackerHandler.BlockingEntity.ProvideComponent<Collider2D>();
                        if (targetCollider != null)
                        {
                            Vector3 closestPoint = targetCollider.ClosestPoint(_mainTargetAttackerHandler.transform.position);
                            if (Vector2.Distance(_mainTargetAttackerHandler.transform.position, closestPoint) <= _mainTargetAttackerData.DistanceToTarget + _mainTargetAttackerSensorData.HitColliderSize)
                            {
                                _damageable = _mainTargetAttackerHandler.BlockingEntity.ProvideComponent<IDamageable>();
                            }
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(_mainTargetAttackerHandler.transform.position, _characterSensor.TargetTransformToAttack.position) <=
                            _mainTargetAttackerHandler.MainTargetAttackerData.DistanceToTarget + _mainTargetAttackerSensorData.HitColliderSize)
                        {
                            _damageable = _characterSensor.TargetToAttack.ProvideComponent<IDamageable>();
                        }
                    }

                    Attack();
                    _attackCooldownTimer = _mainTargetAttackerData.AttackCooldown;
                }
            }
        }

        private void Attack()
        {
            if (_damageable == null)
                return;
            _animator.SetTrigger(AnimatorHashes.SpikeAttackTrigger);
            if (_mainTargetAttackerHandler.CanAttack)
            {
                _damageable?.TakeDamage(_mainTargetAttackerData.Damage);
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

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)_mainTargetAttackerSensorData.CharacterTransform.position + _mainTargetAttackerSensorData.HitColliderOffset, _mainTargetAttackerSensorData.HitColliderSize);

            Gizmos.color = Color.green;
            Gizmos.DrawLine((Vector2)_mainTargetAttackerSensorData.CharacterTransform.position, (Vector2)_characterSensor.TargetTransformToChase.position);

#endif
        }

    }
}
