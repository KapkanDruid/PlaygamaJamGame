using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    class MainTargetAttackerMoveLogic : IDisposable, ITickable
    {
        private NavMeshAgent _agent;
        private ICharacterData _mainTargetAttackerData;
        private ISensorData _mainTargetAttackerSensorData;
        private IEntity _blockingEntity;
        private CharacterSensor _characterSensor;
        private MainTargetAttackerHandler _mainTargetAttackerHandler;
        private bool _hasTarget;
        private bool _isMoving => _agent.velocity.sqrMagnitude > 0.05f && !_agent.isStopped;

        public MainTargetAttackerMoveLogic(MainTargetAttackerHandler mainTargetAttackerHandler, CharacterSensor characterSensor, NavMeshAgent navMeshAgent)
        {
            _mainTargetAttackerHandler = mainTargetAttackerHandler;
            _mainTargetAttackerData = mainTargetAttackerHandler.MainTargetAttackerData;
            _mainTargetAttackerSensorData = (ISensorData)mainTargetAttackerHandler.MainTargetAttackerData;
            _characterSensor = characterSensor;
            _agent = navMeshAgent;

            _characterSensor.TargetDetected += SetTarget;

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
            _mainTargetAttackerHandler.Moving(_isMoving);
        }

        private void MoveToTarget()
        {
            if (_characterSensor.TargetTransformToAttack != null)
                return;

            if (_hasTarget && _mainTargetAttackerHandler.CanMoving)
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

                _blockingEntity = entity;
                return;

            }

            _blockingEntity = null;
        }
    }
}
