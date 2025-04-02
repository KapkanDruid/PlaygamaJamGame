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
        private ObjectPooler<DestroyerEntity> _destroyerPool;
        private List<DestroyerEntity.Factory> _destroyerFactory;
        private DestroyerType _type;
        private GameObject _prefab;
        public DestroyerType Type => _type;

        public class Factory : PlaceholderFactory<DestroyerType, DestroyerSpawner> { }

        public DestroyerSpawner(List<DestroyerEntity.Factory> destroyerFactory, DestroyerType type)
        {
            _destroyerFactory = destroyerFactory;
            _type = type;
        }

        public void Initialize(int capacityInPool)
        {
            foreach (var factory in _destroyerFactory)
            {
                if (factory.Type == _type)
                    _destroyerPool = new ObjectPooler<DestroyerEntity>(capacityInPool, "Destroyers", new InstantiateObjectsByFactory<DestroyerEntity>(factory));
            }
        }

        public Type GetTypeObject()
        {
            return typeof(DestroyerEntity);
        }

        public GameObject GetPrefab()
        {
            var prefab = _destroyerPool.Get();
            _prefab = prefab.gameObject;
            _prefab.transform.position = new(30f, 10f);
            _prefab.SetActive(false);
            return _prefab;
        }

        public void Spawn(Vector3 position)
        {
            _prefab.SetActive(true);
            _prefab.transform.position = position;            
        }
    }
}
