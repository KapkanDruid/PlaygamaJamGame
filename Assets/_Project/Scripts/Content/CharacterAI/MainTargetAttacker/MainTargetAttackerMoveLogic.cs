using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    class MainTargetAttackerMoveLogic : ITickable
    {
        private NavMeshAgent _agent;
        private PauseHandler _pauseHandler;
        private ICharacterData _mainTargetAttackerData;
        private ISensorData _mainTargetAttackerSensorData;
        private IEntity _blockingEntity;
        private CharacterSensor _characterSensor;
        private MainTargetAttackerHandler _mainTargetAttackerHandler;
        private EnemyDeadHandler _enemyDeadHandler;
        private Animator _animator;
        private AnimatorStateInfo _pausedAnimatorState;

        private bool _isMoving => _agent.velocity.sqrMagnitude > 0.05f && !_agent.isStopped;

        public MainTargetAttackerMoveLogic(MainTargetAttackerHandler mainTargetAttackerHandler,
                                           CharacterSensor characterSensor,
                                           NavMeshAgent navMeshAgent,
                                           PauseHandler pauseHandler,
                                           EnemyDeadHandler enemyDeadHandler,
                                           Animator animator)
        {
            _mainTargetAttackerHandler = mainTargetAttackerHandler;
            _mainTargetAttackerData = mainTargetAttackerHandler.MainTargetAttackerData;
            _mainTargetAttackerSensorData = (ISensorData)mainTargetAttackerHandler.MainTargetAttackerData;
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
            _agent.speed = _mainTargetAttackerData.Speed;
            _agent.stoppingDistance = _mainTargetAttackerData.DistanceToTarget;
            _agent.angularSpeed = _mainTargetAttackerData.Speed;
            _blockingEntity = null;
        }

        public void Tick()
        {
            if (_pauseHandler.IsPaused || _enemyDeadHandler.IsDead)
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

            _agent.speed = _mainTargetAttackerData.Speed;
            _agent.isStopped = false;

            if (_characterSensor.TargetToChase == null || !_characterSensor.TargetTransformToChase.gameObject.activeInHierarchy)
                _characterSensor.TargetSearch();

            MoveToTarget();

            SetOrientation();

            _mainTargetAttackerHandler.Moving(_isMoving);
        }

        public void SetOrientation()
        {
            if (_characterSensor.TargetTransformToChase == null)
                return;

            var direction = Mathf.Sign(_characterSensor.TargetTransformToChase.position.x - _mainTargetAttackerHandler.transform.position.x);
            Vector3 rightOrientation = new Vector3(1, _mainTargetAttackerHandler.transform.localScale.y, _mainTargetAttackerHandler.transform.localScale.z);
            Vector3 leftOrientation = new Vector3(-1, _mainTargetAttackerHandler.transform.localScale.y, _mainTargetAttackerHandler.transform.localScale.z);

            if (direction > 0)
            {
                _mainTargetAttackerHandler.transform.localScale = leftOrientation;
            }
            else if (direction < 0)
            {

                _mainTargetAttackerHandler.transform.localScale = rightOrientation;
            }
        }

        private void MoveToTarget()
        {
            if (_characterSensor.TargetToChase != null && _mainTargetAttackerHandler.CanMoving)
            {
                if (_characterSensor.TargetTransformToChase == null)
                    return;

                if (_blockingEntity == null)
                {
                    _mainTargetAttackerHandler.IsPathInvalid(false);
                    _agent.SetDestination(_characterSensor.TargetTransformToChase.position);

                    if (_agent.pathStatus == NavMeshPathStatus.PathPartial)
                    {
                        HandleBlockedPath();
                    }
                }
                else
                {
                    if (_blockingEntity.ProvideComponent<MonoBehaviour>() == null)
                    {
                        _blockingEntity = null;
                        _mainTargetAttackerHandler.IsPathInvalid(false, _blockingEntity);
                        return;
                    }

                    _agent.SetDestination(_blockingEntity.ProvideComponent<MonoBehaviour>().transform.position);
                    _mainTargetAttackerHandler.IsPathInvalid(true, _blockingEntity);
                }

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

        private void HandleBlockedPath()
        {
            _mainTargetAttackerHandler.IsPathInvalid(true);
            Vector2 start = _agent.transform.position;
            Vector2 end = _characterSensor.TargetTransformToChase.position;
            Vector2 direction = (end - start).normalized;
            float distance = Vector2.Distance(start, end);

            RaycastHit2D[] hits = Physics2D.RaycastAll(_agent.transform.position, _characterSensor.TargetTransformToChase.position, distance);

            for (int i = 0; i < hits.Length; i++)
            {
                if (!hits[i].collider.TryGetComponent(out IEntity entity))
                    continue;

                if (entity == _mainTargetAttackerSensorData.ThisEntity)
                    continue;

                Flags flags = entity.ProvideComponent<Flags>();

                if (flags == null)
                    continue;

                if (entity != null)
                {
                    _blockingEntity = entity;
                }
                else
                {
                    _blockingEntity = null;
                }
                return;

            }

        }
    }
}
