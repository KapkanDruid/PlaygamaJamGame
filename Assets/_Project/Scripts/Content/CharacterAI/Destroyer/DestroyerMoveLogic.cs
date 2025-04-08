using Project.Content.CharacterAI.MainTargetAttacker;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerMoveLogic : ITickable
    {
        private NavMeshAgent _agent;
        private ICharacterData _destroyerData;
        private DestroyerEntity _destroyerEntity;
        private PauseHandler _pauseHandler;
        private EnemyDeadHandler _enemyDeadHandler;
        private Animator _animator;

        public DestroyerMoveLogic(DestroyerEntity destroyerEntity,
                                  NavMeshAgent navMeshAgent,
                                  PauseHandler pauseHandler,
                                  EnemyDeadHandler enemyDeadHandler,
                                  Animator animator)
        {
            _destroyerEntity = destroyerEntity;
            _destroyerData = destroyerEntity.DestroyerData;
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
                return;
            }

            _agent.speed = _destroyerData.Speed;
            _agent.isStopped = false;

            if (_enemyDeadHandler.IsDead)
            {
                _agent.speed = 0f;
                _agent.isStopped = true;
                return;
            }

            if (_destroyerEntity.TargetTransform == null)
                return;

            MoveToTarget();
            SetOrientation();
        }

        public void SetOrientation()
        {
            var direction = Mathf.Sign(_destroyerEntity.TargetTransform.position.x - _destroyerEntity.transform.position.x);
            Vector3 orientation = new Vector3(direction, _destroyerEntity.transform.localScale.y, _destroyerEntity.transform.localScale.z);
            _destroyerEntity.transform.localScale = orientation;
        }

        private void MoveToTarget()
        {
            _animator.SetBool(AnimatorHashes.IsMoving, true);
            _agent.SetDestination(_destroyerEntity.TargetTransform.position);
        }

    }
}

