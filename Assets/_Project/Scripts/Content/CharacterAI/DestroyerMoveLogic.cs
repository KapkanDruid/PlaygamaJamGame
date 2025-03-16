using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI
{
    public class DestroyerMoveLogic : IInitializable, IDisposable, ITickable
    {
        private NavMeshAgent _agent;
        private ICharacterData _characterData;
        private CharacterSensor _characterSensor;
        private bool _hasTarget;

        public DestroyerMoveLogic(DestroyerHandler characterHandler, CharacterSensor characterSensor, NavMeshAgent navMeshAgent)
        {
            _characterData = characterHandler.DestroyerData;
            _characterSensor = characterSensor;
            _agent = navMeshAgent;

            _characterSensor.TargetDetected += SetTarget;
        }

        public void Initialize()
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = _characterData.Speed;
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
            if (_hasTarget)
            {
                _agent.SetDestination(_characterSensor.TargetTransform.position);
            }
        }
    }
}

