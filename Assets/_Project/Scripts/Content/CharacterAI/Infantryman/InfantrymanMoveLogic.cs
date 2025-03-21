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
        private Vector3 _currentPatrolPoint;
        private bool _isPatrolling;

        public InfantrymanMoveLogic(InfantrymanEntity infantrymanEntity, NavMeshAgent agent, PatrolLogic patrolLogic)
        {
            _infantrymanEntity = infantrymanEntity;
            _infantrymanData = infantrymanEntity.InfantrymanData;
            _agent = agent;
            _patrolLogic = patrolLogic;

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
            if (_infantrymanEntity.TargetTransform == null)
            {
                Patrol();
                return;
            }

            MoveToTarget();
        }

        private void Patrol()
        {
            if (!_isPatrolling || HasReachedPatrolPoint())
            {
                _currentPatrolPoint = _patrolLogic.GetRandomPoint(_infantrymanEntity.FlagTransform.position, _agent.stoppingDistance * 2);
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

