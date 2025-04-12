using Project.Content.BuildSystem;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    class MainTargetAttackerMoveLogic : ITickable
    {
        private NavMeshAgent _agent;
        private PauseHandler _pauseHandler;
        private ICharacterData _characterData;
        private MainTargetAttackerData _mainTargetAttackerData;
        private ISensorData _mainTargetAttackerSensorData;
        private IEntity _blockingEntity;
        private MainTargetAttackerEntity _mainTargetAttackerEntity;
        private EnemyDeadHandler _enemyDeadHandler;
        private Animator _animator;

        public MainTargetAttackerMoveLogic(MainTargetAttackerEntity mainTargetAttackerEntity,
                                           NavMeshAgent navMeshAgent,
                                           PauseHandler pauseHandler,
                                           EnemyDeadHandler enemyDeadHandler,
                                           Animator animator,
                                           ICharacterData characterData,
                                           MainTargetAttackerData mainTargetAttackerData)
        {
            _mainTargetAttackerEntity = mainTargetAttackerEntity;
            _characterData = characterData;
            _mainTargetAttackerData = mainTargetAttackerData;
            _mainTargetAttackerSensorData = _mainTargetAttackerData.SensorData;
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
            _agent.speed = _characterData.Speed;
            _agent.stoppingDistance = _characterData.DistanceToTarget;
            _agent.angularSpeed = _characterData.Speed;
            _blockingEntity = null;
        }

        public void Tick()
        {
            if (_pauseHandler.IsPaused)
            {
                _agent.speed = 0f;
                _agent.isStopped = true;
                return;
            }

            _agent.speed = _characterData.Speed;
            _agent.isStopped = false;

            if (_enemyDeadHandler.IsDead)
            {
                _agent.speed = 0f;
                _agent.isStopped = true;
                return;
            }

            if (_mainTargetAttackerEntity.TargetTransform == null)
                return;

            MoveToTarget();

            SetOrientation();
        }

        public void SetOrientation()
        {
            var direction = Mathf.Sign(_mainTargetAttackerEntity.TargetTransform.position.x - _mainTargetAttackerEntity.transform.position.x);
            Vector3 orientation = new Vector3(direction * -1, _mainTargetAttackerEntity.transform.localScale.y, _mainTargetAttackerEntity.transform.localScale.z);
            _mainTargetAttackerEntity.transform.localScale = orientation;
        }

        private void MoveToTarget()
        {
            _animator.SetBool(AnimatorHashes.IsMoving, true);

            if (_blockingEntity == null)
            {
                _mainTargetAttackerEntity.IsPathInvalid(false);
                _agent.SetDestination(_mainTargetAttackerEntity.TargetTransform.position);

                if (_agent.pathStatus == NavMeshPathStatus.PathPartial)
                {
                    HandleBlockedPath();
                }
            }
            else
            {
                HandleBlockingEntity();
            }
        }

        private void HandleBlockedPath()
        {
            _mainTargetAttackerEntity.IsPathInvalid(true);
            Vector2 start = _agent.transform.position;
            Vector2 end = _mainTargetAttackerEntity.TargetTransform.position;
            Vector2 direction = (end - start).normalized;
            float distance = Vector2.Distance(start, end);

            RaycastHit2D[] hits = Physics2D.RaycastAll(_agent.transform.position, _mainTargetAttackerEntity.TargetTransform.position, distance);

            for (int i = 0; i < hits.Length; i++)
            {
                if (!hits[i].collider.TryGetComponent(out IEntity entity))
                    continue;

                if (entity == _mainTargetAttackerEntity.MainTargetAttackerData)
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

        private void HandleBlockingEntity()
        {
            if (_blockingEntity.ProvideComponent<MonoBehaviour>() == null)
            {
                _blockingEntity = null;
                _mainTargetAttackerEntity.IsPathInvalid(false, _blockingEntity);
                return;
            }

            _agent.SetDestination(_blockingEntity.ProvideComponent<MonoBehaviour>().transform.position);
            _mainTargetAttackerEntity.IsPathInvalid(true, _blockingEntity);
        }
    }
}
