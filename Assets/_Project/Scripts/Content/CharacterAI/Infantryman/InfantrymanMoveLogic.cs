using UnityEngine.AI;
using UnityEngine;
using Zenject;

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

        public InfantrymanMoveLogic(InfantrymanEntity infantrymanEntity,
                                    NavMeshAgent agent,
                                    PatrolLogic patrolLogic,
                                    PauseHandler pauseHandler)
        {
            _infantrymanEntity = infantrymanEntity;
            _infantrymanData = infantrymanEntity.InfantrymanData;
            _agent = agent;
            _patrolLogic = patrolLogic;
            _pauseHandler = pauseHandler;

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
                return;

            _agent.isStopped = _pauseHandler.IsPaused;

            if (_infantrymanEntity.TargetTransform == null)
            {
                if (_infantrymanEntity.FlagTransform == null)
                    return;

                Patrol();
                return;
            }

            MoveToTarget();
            SetOrientation();
        }

        public void SetOrientation()
        {
            if (_infantrymanEntity.TargetTransform == null)
                return;

            var direction = Mathf.Sign(_infantrymanEntity.TargetTransform.position.x - _infantrymanEntity.transform.position.x);
            Vector3 rightOrientation = new Vector3(1, _infantrymanEntity.transform.localScale.y, _infantrymanEntity .transform.localScale.z);
            Vector3 leftOrientation = new Vector3(-1, _infantrymanEntity.transform.localScale.y, _infantrymanEntity.transform.localScale.z);

            if (direction > 0)
            {
                _infantrymanEntity.transform.localScale = rightOrientation;
            }
            else if (direction < 0)
            {
                _infantrymanEntity.transform.localScale = leftOrientation;
            }
        }

        private void Patrol()
        {
            if (!_isPatrolling || HasReachedPatrolPoint())
            {
                _currentPatrolPoint = _patrolLogic.GetRandomPoint(_infantrymanEntity.FlagTransform.position, _infantrymanEntity.PatrolRadius + _agent.stoppingDistance);
                _agent.SetDestination(_currentPatrolPoint);
                _isPatrolling = true;
            }
        }

        private bool HasReachedPatrolPoint()
        {
            return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
        }

        private void MoveToTarget()
        {
            _agent.SetDestination(_infantrymanEntity.TargetTransform.position);
            _isPatrolling = false;
        }
    }
}

