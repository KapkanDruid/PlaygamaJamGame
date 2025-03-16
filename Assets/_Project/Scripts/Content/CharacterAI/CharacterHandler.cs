using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI
{
    public abstract class CharacterHandler : MonoBehaviour, IEntity
    {
        protected EnemyDeadHandler _enemyDeadHandler;
        protected CancellationToken _cancellationToken;

        public CancellationToken CancellationToken => _cancellationToken;

        public virtual T ProvideComponent<T>() where T : class
        {
            return null;            
        }

    }
}

