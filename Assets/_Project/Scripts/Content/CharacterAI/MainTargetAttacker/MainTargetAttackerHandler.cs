using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    public class MainTargetAttackerHandler : CharacterHandler
    {
        [SerializeField] private MainTargetAttackerData _mainTargetAttackerData;
        private bool _canAttack;
        private bool _canMoving;
        private bool _isPathInvalid;
        private CharacterSensor _characterSensor;
        private IEntity _blockingEntity;
        private bool _isMoving;

        public ICharacterData MainTargetAttackerData => _mainTargetAttackerData;
        public IEntity BlockingEntity => _blockingEntity;

        public bool CanAttack => _canAttack;
        public bool CanMoving => _canMoving;
        public bool PathInvalid => _isPathInvalid;
        public bool IsMoving => _isMoving;

        public event Action PathBlocked;

        public class Factory : PlaceholderFactory<MainTargetAttackerHandler> { }

        [Inject]
        public void Construct(CharacterHealthHandler healthHandler, EnemyDeadHandler enemyDeadHandler, CharacterSensor characterSensor)
        {
            _mainTargetAttackerData.ThisEntity = this;
            _healthHandler = healthHandler;
            _enemyDeadHandler = enemyDeadHandler;
            _characterSensor = characterSensor;

            _characterSensor.TargetDetected += HasTarget;
            _characterSensor.HasTargetToAttack += HasTargetToAttack;
            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        public override T ProvideComponent<T>() where T : class
        {
            if (_mainTargetAttackerData.Flags is T flags)
                return flags;

            if (_healthHandler is T healthHandler)
                return healthHandler;

            if (transform is T characterTransform)
                return characterTransform;

            if (_enemyDeadHandler is T deadHandler)
                return deadHandler;

            return null;
        }

        public void IsPathInvalid(bool isInvalid, IEntity blockingEntity = null)
        {
            _isPathInvalid = isInvalid;
            if (isInvalid)
            {
                PathBlocked?.Invoke();
                _blockingEntity = blockingEntity;
            }
        }

        public void Moving(bool isMoving)
        {
            _isMoving = isMoving;
            _canAttack = !isMoving;
        }

        private void Start()
        {
            _healthHandler.Initialize();
        }

        private void HasTarget()
        {
            _canMoving = true;
        }

        private void HasTargetToAttack()
        {
            _canAttack = true;
        }

        private void OnDestroy()
        {
            _characterSensor.TargetDetected -= HasTarget;
            _characterSensor.HasTargetToAttack -= HasTargetToAttack;
        }
    }
}