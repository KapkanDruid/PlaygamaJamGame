using Project.Content.CharacterAI.MainTargetAttacker;
using System;
using UnityEngine;

namespace Project.Content.Spawners
{
    public class MainTargetAttackerSpawner : IEnemySpawner
    {
        private ObjectPooler<MainTargetAttackerHandler> _mainTargetAttackerPool;
        private MainTargetAttackerHandler.Factory _mainTargetAttackerFactory;

        public MainTargetAttackerSpawner(MainTargetAttackerHandler.Factory mainTargetAttackerFactory)
        {
            _mainTargetAttackerFactory = mainTargetAttackerFactory;
        }
        public void Initialize(int capacityInPool)
        {
            _mainTargetAttackerPool = new ObjectPooler<MainTargetAttackerHandler>(capacityInPool, "MainTargetAttackers", new InstantiateObjectsByFactory<MainTargetAttackerHandler>(_mainTargetAttackerFactory));
        }
        public Type GetTypeObject()
        {
            return typeof(MainTargetAttackerHandler);
        }
        public void Spawn(Vector3 position)
        {
            var prefab = _mainTargetAttackerPool.Get();
            prefab.transform.position = position;
        }
    }
}
