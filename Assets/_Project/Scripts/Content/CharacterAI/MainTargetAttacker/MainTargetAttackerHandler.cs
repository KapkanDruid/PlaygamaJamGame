using Cysharp.Threading.Tasks;
using Project.Content.CharacterAI.Destroyer;
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
        private Animator _animator;
        private LevelExperienceController _levelExperience;
        private IEntity _blockingEntity;
        private bool _isMoving;
        private FloatingTextHandler _textHandler;

        public ICharacterData MainTargetAttackerData => _mainTargetAttackerData;
        public IEntity BlockingEntity => _blockingEntity;

        public bool CanAttack => _canAttack;
        public bool CanMoving => _canMoving;
        public bool PathInvalid => _isPathInvalid;
        public bool IsMoving => _isMoving;

        public event Action PathBlocked;

        public class Factory : PlaceholderFactory<MainTargetAttackerHandler> 
        {
            public readonly MainTargetAttackerType Type;

            public Factory(MainTargetAttackerType type) : base()
            {
                Type = type;
            }
        }

        [Inject]
        public void Construct(EnemyDeadHandler enemyDeadHandler,
                              CharacterSensor characterSensor,
                              Animator animator,
                              LevelExperienceController levelExperience,
                              FloatingTextHandler textHandler)
        {
            _mainTargetAttackerData.ThisEntity = this;
            _enemyDeadHandler = enemyDeadHandler;
            _characterSensor = characterSensor;
            _animator = animator;
            _levelExperience = levelExperience;
            _textHandler = textHandler;

            ResetData();
            _characterSensor.TargetDetected += HasTarget;
            _characterSensor.HasTargetToAttack += HasTargetToAttack;
            _enemyDeadHandler.OnDeath += DropExperience;
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
            _animator.SetBool(AnimatorHashes.IsMoving, _isMoving);
            _canAttack = !isMoving;
        }

        private void DropExperience()
        {
            _levelExperience.OnEnemyDied(_mainTargetAttackerData.CharacterTransform.position, _mainTargetAttackerData.ExperiencePoints);
        }

        private void OnEnable()
        {
            ResetData();
            _characterSensor.ResetTargets();
            _animator.Rebind();
            _animator.Update(0f);
            _enemyDeadHandler.Reset();
            _healthHandler.Reset();
        }

        private void ResetData()
        {
            _healthHandler = new CharacterHealthHandler(_mainTargetAttackerData.Health, _animator, _enemyDeadHandler);

            if (_mainTargetAttackerData.FloatingText != null)
                _healthHandler.OnDamage += (damage) => _textHandler.ShowText(_mainTargetAttackerData.FloatingText, _mainTargetAttackerData.CharacterTransform.position, damage.ToString());
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
            _enemyDeadHandler.OnDeath -= DropExperience;
        }
    }
}