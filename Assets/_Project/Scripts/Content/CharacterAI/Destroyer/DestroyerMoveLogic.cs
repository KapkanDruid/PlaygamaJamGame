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

        public DestroyerMoveLogic(DestroyerHandler destroyerHandler, CharacterSensor characterSensor, NavMeshAgent navMeshAgent)
        {
            _destroyerHandler = destroyerHandler;
            _destroyerData = destroyerHandler.DestroyerData;
            _characterSensor = characterSensor;
            _agent = navMeshAgent;

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
            if (_characterSensor.TargetToChase == null || !_characterSensor.TargetTransformToChase.gameObject.activeInHierarchy)
                _characterSensor.TargetSearch();

            MoveToTarget();
        }

        private void MoveToTarget()
        {
            if (_characterSensor.TargetToChase != null && _destroyerHandler.CanMoving)
            {
                if (_characterSensor.TargetTransformToChase == null)
                    return;

                _agent.SetDestination(_characterSensor.TargetTransformToChase.position);
            }
        }
    }
}

