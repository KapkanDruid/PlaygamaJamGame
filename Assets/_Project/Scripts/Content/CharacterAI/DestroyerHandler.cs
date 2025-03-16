using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI
{
    public class DestroyerHandler : CharacterHandler
    {
        [SerializeField] private DestroyerData _destroyerData;

        protected CharacterHealthHandler _healthHandler;
        public ICharacterData DestroyerData => _destroyerData;

        [Inject]
        public void Construct(CharacterHealthHandler healthHandler, EnemyDeadHandler enemyDeadHandler)
        {
            _destroyerData.ThisEntity = this;
            _healthHandler = healthHandler;
            _enemyDeadHandler = enemyDeadHandler;

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

        private void Start()
        {
            _healthHandler.Initialize();
        }
    }
}

