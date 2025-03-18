using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    class MainTargetAttackerMoveLogic : IDisposable, ITickable
    {
        private NavMeshAgent _agent;
        private NavMeshPath _path = new NavMeshPath();
        private ICharacterData _mainTargetAttackerData;
        private CharacterSensor _characterSensor;
        private MainTargetAttackerHandler _mainTargetAttackerHandler;
        private bool _hasTarget;

        public MainTargetAttackerMoveLogic(MainTargetAttackerHandler mainTargetAttackerHandler, CharacterSensor characterSensor, NavMeshAgent navMeshAgent)
        {
            _mainTargetAttackerHandler = mainTargetAttackerHandler;
            _mainTargetAttackerData = mainTargetAttackerHandler.MainTargetAttackerData;
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

            if (_hasTarget && _mainTargetAttackerHandler.CanMoving)
            {
                if (_characterSensor.TargetTransformToChase == null)
                    return;

                bool hasPath = NavMesh.CalculatePath(_agent.transform.position, _characterSensor.TargetTransformToChase.position, NavMesh.AllAreas, _path);

                Debug.Log($"Есть путь к главному зданию: {hasPath}");
                if (!hasPath)
                {
                    Debug.Log("Путь к цели невозможен!");
                    HandleBlockedPath();
                }
                else
                {
                    _mainTargetAttackerHandler.IsPathInvalid(false);
                    _agent.SetDestination(_characterSensor.TargetTransformToChase.position);
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
                if (hits[i].collider != null && hits[i].collider.gameObject != _agent.gameObject)
                {
                    Debug.Log($"Путь заблокирован объектом на позиции: {hits[i].collider}");
                    _agent.SetDestination(hits[i].collider.transform.position);
                    break;
                }
            }
        }
    }
}
