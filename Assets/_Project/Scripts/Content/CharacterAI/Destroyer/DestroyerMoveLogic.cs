using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerMoveLogic : ITickable
    {
        private NavMeshAgent _agent;
        private ICharacterData _destroyerData;
        private CharacterSensor _characterSensor;
        private DestroyerHandler _destroyerHandler;
        private PauseHandler _pauseHandler;
        private EnemyDeadHandler _enemyDeadHandler;
        private Animator _animator;
        private AnimatorStateInfo _pausedAnimatorState;

        public DestroyerMoveLogic(DestroyerHandler destroyerHandler,
                                  CharacterSensor characterSensor,
                                  NavMeshAgent navMeshAgent,
                                  PauseHandler pauseHandler,
                                  EnemyDeadHandler enemyDeadHandler,
                                  Animator animator)
        {
            _destroyerHandler = destroyerHandler;
            _destroyerData = destroyerHandler.DestroyerData;
            _characterSensor = characterSensor;
            _agent = navMeshAgent;
            _pauseHandler = pauseHandler;
            _enemyDeadHandler = enemyDeadHandler;
            _animator = animator;

            Initialize();
        }

        public void Initialize()
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = _destroyerData.Speed;
            _agent.stoppingDistance = _destroyerData.DistanceToTarget;
            _agent.angularSpeed = _destroyerData.Speed;
        }

        public void Tick()
        {
            if (_pauseHandler.IsPaused)
            {
                _agent.speed = 0f;
                _agent.isStopped = true;
                PauseAnimation();
                return;
            }
            else
            {
                ResumeAnimation();
            }

            _agent.speed = _destroyerData.Speed;
            _agent.isStopped = false;

            if (_characterSensor.TargetToChase == null || _characterSensor.TargetTransformToChase == null)
            {
                _characterSensor.TargetSearch();

                if (_characterSensor.TargetTransformToChase != null)
                {
                    if (!_characterSensor.TargetTransformToChase.gameObject.activeInHierarchy)
                        _characterSensor.ScanAreaToAttack();
                }
            }

            MoveToTarget();
            SetOrientation();
        }

        public void SetOrientation()
        {
            if (_characterSensor.TargetTransformToChase == null)
                return;

            var direction = Mathf.Sign(_characterSensor.TargetTransformToChase.position.x - _destroyerHandler.transform.position.x);
            Vector3 rightOrientation = new Vector3(1, _destroyerHandler.transform.localScale.y, _destroyerHandler.transform.localScale.z);
            Vector3 leftOrientation = new Vector3(-1, _destroyerHandler.transform.localScale.y, _destroyerHandler.transform.localScale.z);

            if (direction > 0)
            {
                _destroyerHandler.transform.localScale = rightOrientation;
            }
            else if (direction < 0)
            {
                _destroyerHandler.transform.localScale = leftOrientation;
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

        private void MoveToTarget()
        {
            if (_characterSensor.TargetToChase != null && _destroyerHandler.IsMoving)
            {
                if (_characterSensor.TargetTransformToChase == null)
                    return;

                _agent.SetDestination(_characterSensor.TargetTransformToChase.position);
            }
        }

    }
}

