using Project.Content.CharacterAI.Destroyer;
using System;
using UnityEngine;

namespace Project.Content.Spawners
{
    public class DestroyerSpawner : IEnemySpawner
    {
        private ObjectPooler<DestroyerHandler> _destroyerPool;
        private DestroyerHandler.Factory _destroyerFactory;

        public DestroyerSpawner(DestroyerHandler.Factory destroyerFactory)
        {
            _destroyerFactory = destroyerFactory;
        }

        public void Initialize(int capacityInPool)
        {
            _destroyerPool = new ObjectPooler<DestroyerHandler>(capacityInPool, "Destroyers", new InstantiateObjectsByFactory<DestroyerHandler>(_destroyerFactory));
        }

        public Type GetTypeObject()
        {
            return typeof(DestroyerHandler);
        }

        public void Spawn(Vector3 position)
        {
            var prefab = _destroyerPool.Get();
            prefab.transform.position = position;
        }
    }
}
