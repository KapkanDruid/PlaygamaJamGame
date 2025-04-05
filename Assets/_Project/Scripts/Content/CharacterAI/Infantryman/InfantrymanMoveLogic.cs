using UnityEngine.AI;
using UnityEngine;
using Zenject;
using Project.Content.CharacterAI.MainTargetAttacker;
using System.Linq;

namespace Project.Content.CharacterAI.Infantryman
{
    public class InfantrymanMoveLogic : ITickable
    {
        private NavMeshAgent _agent;
        private IAllyEntityData _infantrymanData;
        private InfantrymanEntity _infantrymanEntity;
        private PatrolLogic _patrolLogic;
        private PauseHandler _pauseHandler;
        private Vector3 _currentPatrolPoint;
        private bool _isPatrolling;
        private Animator _animator;
        private EnemyDeadHandler _enemyDeadHandler;


        public InfantrymanMoveLogic(InfantrymanEntity infantrymanEntity,
                                    NavMeshAgent agent,
                                    PatrolLogic patrolLogic,
                                    PauseHandler pauseHandler,
                                    Animator animator,
                                    EnemyDeadHandler enemyDeadHandler)
        {
            _infantrymanEntity = infantrymanEntity;
            _infantrymanData = infantrymanEntity.InfantrymanData;
            _agent = agent;
            _patrolLogic = patrolLogic;
            _pauseHandler = pauseHandler;
            _animator = animator;
            _enemyDeadHandler = enemyDeadHandler;

            ConfiguringAgent();
        }

        private void ConfiguringAgent()
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = _infantrymanData.Speed;
            _agent.stoppingDistance = _infantrymanData.AttackRange;
            _agent.angularSpeed = _infantrymanData.Speed;
        }

        public void Tick()
        {
            if (_pauseHandler.IsPaused)
            {
                _agent.speed = 0f;
                _agent.isStopped = true;
                return;
            }

            _agent.speed = _infantrymanData.Speed;
            _agent.isStopped = false;

            if (_enemyDeadHandler.IsDead)
                return;

            if (_infantrymanEntity.TargetTransform == null)
            {
                if (_infantrymanEntity.FlagTransform == null)
                    return;

                Patrol();

                return;
            }

            MoveToTarget();
        }

        public void SetOrientation(float lookAt)
        {
            var direction = Mathf.Sign(lookAt - _infantrymanEntity.transform.position.x);
            Vector3 orientation = new Vector3(direction, _infantrymanEntity.transform.localScale.y, _infantrymanEntity.transform.localScale.z);
            _infantrymanEntity.transform.localScale = orientation;
        }

        private void Patrol()
        {
            if (!_isPatrolling || HasReachedPatrolPoint())
            {
                _currentPatrolPoint = _patrolLogic.GetRandomPoint(_infantrymanEntity.FlagTransform.position, _infantrymanEntity.PatrolRadius + _agent.stoppingDistance);
                _agent.SetDestination(_currentPatrolPoint);
                SetOrientation(_currentPatrolPoint.x);
                _isPatrolling = true;
            }
        }

        private bool HasReachedPatrolPoint()
        {
            return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
        }

        private void MoveToTarget()
        {
            _animator.SetBool(AnimatorHashes.IsMoving, true);
            _agent.SetDestination(_infantrymanEntity.TargetTransform.position);
            SetOrientation(_infantrymanEntity.TargetTransform.position.x);
            _isPatrolling = false;
        }
    }
}

