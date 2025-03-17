using System;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerMoveLogic : IInitializable, IDisposable, ITickable
    {
        private NavMeshAgent _agent;
        private ICharacterData _destroyerData;
        private CharacterSensor _characterSensor;
        private DestroyerHandler _destroyerHandler;
        private bool _hasTarget;

        public DestroyerMoveLogic(DestroyerHandler characterHandler, CharacterSensor characterSensor, NavMeshAgent navMeshAgent)
        {
            _destroyerHandler = characterHandler;
            _destroyerData = characterHandler.DestroyerData;
            _characterSensor = characterSensor;
            _agent = navMeshAgent;

            _characterSensor.TargetDetected += SetTarget;
        }

        public void Initialize()
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = _destroyerData.Speed;
        }

        private void SetTarget()
        {
            _hasTarget = true;
        }

        public void Dispose()
        {
            _characterSensor.TargetDetected -= SetTarget;
        }

        public void Tick()
        {
            if (_hasTarget && _destroyerHandler.CanMoving)
            {
                _agent.SetDestination(_characterSensor.TargetTransformToChase.position);
            }
        }
    }
}

