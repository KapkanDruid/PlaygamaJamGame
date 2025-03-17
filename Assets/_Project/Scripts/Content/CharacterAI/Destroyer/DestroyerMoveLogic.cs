using System;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerMoveLogic : IDisposable, ITickable
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
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            if (_characterSensor.TargetTransformToAttack != null)
                return;

            if (_hasTarget && _destroyerHandler.CanMoving)
            {
                if (_characterSensor.TargetTransformToChase == null)
                    return;

                _agent.SetDestination(_characterSensor.TargetTransformToChase.position);
            }
        }
    }
}

