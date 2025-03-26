using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerHandler : CharacterHandler
    {
        [SerializeField] private DestroyerData _destroyerData;
        private bool _canAttack;
        private bool _isMoving;
        private CharacterSensor _characterSensor;
        private LevelExperienceController _levelExperience;
        private Animator _animator;
        private FloatingTextHandler _textHandler;

        public ICharacterData DestroyerData => _destroyerData;

        public bool CanAttack => _canAttack;
        public bool IsMoving => _isMoving;

        public class Factory : PlaceholderFactory<DestroyerHandler> 
        {
            public readonly DestroyerType Type; 
            
            public Factory(DestroyerType type) : base()
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
            _destroyerData.ThisEntity = this;
            _animator = animator;
            _enemyDeadHandler = enemyDeadHandler;
            _characterSensor = characterSensor;
            _levelExperience = levelExperience;
            _textHandler = textHandler;

            ResetData();
            _characterSensor.TargetDetected += HasTarget;
            _enemyDeadHandler.OnDeath += DropExperience;
            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        public override T ProvideComponent<T>() where T : class
        {
            if (_destroyerData.Flags is T flags)
                return flags;

            if (_healthHandler is T healthHandler)
                return healthHandler;

            if (transform is T characterTransform)
                return characterTransform;

            if (_enemyDeadHandler is T deadHandler)
                return deadHandler;

            return null;
        }

        private void DropExperience()
        {
            _levelExperience.OnEnemyDied(_destroyerData.CharacterTransform.position, _destroyerData.ExperiencePoints);
        }

        private void Update()
        {
            _isMoving = true;
            _animator.SetBool(AnimatorHashes.IsMoving, _isMoving);
        }

        private void OnEnable()
        {
            ResetData();
            _animator.Rebind();
            _animator.Update(0f);
            _enemyDeadHandler.Reset();
            _healthHandler.Reset();
        }

        private void ResetData()
        {
            _healthHandler = new CharacterHealthHandler(_destroyerData.Health, _animator, _enemyDeadHandler);

            if (_destroyerData.FloatingText != null)
                _healthHandler.OnDamage += (damage) => _textHandler.ShowText(_destroyerData.FloatingText, _destroyerData.CharacterTransform.position, damage.ToString());
        }

        private void HasTarget()
        {
            
        }

        private void OnDestroy()
        {
            _characterSensor.TargetDetected -= HasTarget;
            _enemyDeadHandler.OnDeath -= DropExperience;
        }
    }
}

