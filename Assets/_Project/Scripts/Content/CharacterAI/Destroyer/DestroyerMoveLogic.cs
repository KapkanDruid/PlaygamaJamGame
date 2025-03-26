using UnityEngine;
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
        private PauseHandler _pauseHandler;
        private EnemyDeadHandler _enemyDeadHandler;

        public DestroyerMoveLogic(DestroyerHandler destroyerHandler,
                                  CharacterSensor characterSensor,
                                  NavMeshAgent navMeshAgent,
                                  PauseHandler pauseHandler,
                                  EnemyDeadHandler enemyDeadHandler)
        {
            _destroyerHandler = destroyerHandler;
            _destroyerData = destroyerHandler.DestroyerData;
            _characterSensor = characterSensor;
            _agent = navMeshAgent;
            _pauseHandler = pauseHandler;
            _enemyDeadHandler = enemyDeadHandler;

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
            if (_pauseHandler.IsPaused || _enemyDeadHandler.IsDead)
            {
                _agent.speed = 0f;
                _agent.isStopped = true;
                return;
            }

            _agent.speed = _destroyerData.Speed;
            _agent.isStopped = false;

            if (_characterSensor.TargetToChase == null || !_characterSensor.TargetTransformToChase.gameObject.activeInHierarchy)
                _characterSensor.TargetSearch();

            MoveToTarget();
            SetOrientation();
        }

        public void SetOrientation()
        {
            if (_characterSensor.TargetTransformToChase == null)
                return;

            var direction = Mathf.Sign(_characterSensor.TargetTransformToChase.position.x - _destroyerHandler.transform.position.x);
            Vector3 rightOrientation = new Vector3(1, _destroyerHandler.transform.localScale.y, _destroyerHandler.transform.localScale.z);
            Vector3 leftOrientation = new Vector3(-1, _destroyerHandler.transform.localScale.y, _destroyerHandler.transform.localScale.z);

            if (direction > 0)
            {
                _destroyerHandler.transform.localScale = rightOrientation;
            }
            else if (direction < 0)
            {
                _destroyerHandler.transform.localScale = leftOrientation;
            }
        }

        private void MoveToTarget()
        {
            if (_characterSensor.TargetToChase != null && _destroyerHandler.IsMoving)
            {
                if (_characterSensor.TargetTransformToChase == null)
                    return;

                _agent.SetDestination(_characterSensor.TargetTransformToChase.position);
            }
        }

    }
}

