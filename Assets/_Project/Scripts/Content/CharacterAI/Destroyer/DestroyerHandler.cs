using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerHandler : CharacterHandler
    {
        [SerializeField] private DestroyerData _destroyerData;
        private bool _canAttack;
        private bool _canMoving;
        private CharacterSensor _characterSensor;
        private Animator _animator;

        public ICharacterData DestroyerData => _destroyerData;

        public bool CanAttack => _canAttack;
        public bool CanMoving => _canMoving;

        public class Factory : PlaceholderFactory<DestroyerHandler> 
        {
            public readonly DestroyerType Type; 
            
            public Factory(DestroyerType type) : base()
            {
                Type = type;
            }
        }

        [Inject]
        public void Construct(EnemyDeadHandler enemyDeadHandler, CharacterSensor characterSensor, Animator animator)
        {
            _destroyerData.ThisEntity = this;
            _animator = animator;
            _enemyDeadHandler = enemyDeadHandler;
            _characterSensor = characterSensor;

            ResetData();
            _characterSensor.TargetDetected += HasTarget;
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
        }

        private void HasTarget()
        {
            _canMoving = true;
        }

        private void OnDestroy()
        {
            _characterSensor.TargetDetected -= HasTarget;
        }
    }
}

