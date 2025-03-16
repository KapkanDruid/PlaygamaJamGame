using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI
{
    public class DestroyerHandler : MonoBehaviour, IEntity
    {
        [SerializeField] private DestroyerData _destroyerData;

        protected CharacterHealthHandler _healthHandler;
        protected EnemyDeadHandler _enemyDeadHandler;
        protected CancellationToken _cancellationToken;

        public CancellationToken CancellationToken => _cancellationToken;
        public ICharacterData DestroyerData => _destroyerData;

        [Inject]
        public void Construct(CharacterHealthHandler healthHandler, EnemyDeadHandler enemyDeadHandler)
        {
            _destroyerData.ThisEntity = this;
            _healthHandler = healthHandler;
            _enemyDeadHandler = enemyDeadHandler;

            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        public T ProvideComponent<T>() where T : class
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

