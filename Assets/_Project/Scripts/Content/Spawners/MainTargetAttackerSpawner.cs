using Project.Content.CharacterAI;
using Project.Content.CharacterAI.MainTargetAttacker;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.Spawners
{
    public class MainTargetAttackerSpawner : IEnemySpawner
    {
        private ObjectPooler<MainTargetAttackerHandler> _mainTargetAttackerPool;
        private List<MainTargetAttackerHandler.Factory> _mainTargetAttackerFactory;
        private MainTargetAttackerType _type;

        public class Factory : PlaceholderFactory<MainTargetAttackerType, MainTargetAttackerSpawner> { }

        public MainTargetAttackerSpawner(List<MainTargetAttackerHandler.Factory> mainTargetAttackerFactory, MainTargetAttackerType type)
        {
            _mainTargetAttackerFactory = mainTargetAttackerFactory;
            _type = type;
        }

        public void Initialize(int capacityInPool)
        {
            foreach (var factory in _mainTargetAttackerFactory)
            {
                if (factory.Type == _type)
                    _mainTargetAttackerPool = new ObjectPooler<MainTargetAttackerHandler>(capacityInPool, "MainTargetAttackers", new InstantiateObjectsByFactory<MainTargetAttackerHandler>(factory));
            }
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
