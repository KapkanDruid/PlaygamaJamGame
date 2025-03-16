using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Content.Character
{
    public class CharacterHandler : MonoBehaviour, IEntity
    {
        [SerializeField] private CharacterData _data;

        private CharacterHealthHandler _healthHandler;
        private EnemyDeadHandler _enemyDeadHandler;
        private CancellationToken _cancellationToken;

        public CancellationToken CancellationToken => _cancellationToken;
        public CharacterData CharacterData => _data;


        [Inject]
        public void Construct(CharacterHealthHandler healthHandler, EnemyDeadHandler enemyDeadHandler)
        {
            _healthHandler = healthHandler;
            _data.ThisEntity = this;
            _enemyDeadHandler = enemyDeadHandler;

            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        public T ProvideComponent<T>() where T : class
        {
            if (_data.Flags is T flags)
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

