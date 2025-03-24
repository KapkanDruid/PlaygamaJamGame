using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Destroyer;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.Spawners
{
    public class DestroyerSpawner : IEnemySpawner
    {
        private ObjectPooler<DestroyerHandler> _destroyerPool;
        private List<DestroyerHandler.Factory> _destroyerFactory;
        private DestroyerType _type;
        private GameObject _prefab;

        public GameObject Prefab => _prefab;

        public class Factory : PlaceholderFactory<DestroyerType, DestroyerSpawner> { }

        public DestroyerSpawner(List<DestroyerHandler.Factory> destroyerFactory, DestroyerType type)
        {
            _destroyerFactory = destroyerFactory;
            _type = type;
        }

        public void Initialize(int capacityInPool)
        {
            foreach (var factory in _destroyerFactory)
            {
                if (factory.Type == _type)
                    _destroyerPool = new ObjectPooler<DestroyerHandler>(capacityInPool, "Destroyers", new InstantiateObjectsByFactory<DestroyerHandler>(factory));
            }
        }

        public Type GetTypeObject()
        {
            return typeof(DestroyerHandler);
        }

        public void Spawn(Vector3 position)
        {
            var prefab = _destroyerPool.Get();
            prefab.transform.position = position;

            _prefab = prefab.gameObject;
        }
    }
}
