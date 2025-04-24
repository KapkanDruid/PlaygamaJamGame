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
        private MonoObjectPooler<MainTargetAttackerEntity> _mainTargetAttackerPool;
        private List<MainTargetAttackerEntity.Factory> _mainTargetAttackerFactory;
        private MainTargetAttackerType _type;
        private GameObject _prefab;

        public class Factory : PlaceholderFactory<MainTargetAttackerType, MainTargetAttackerSpawner> { }

        public MainTargetAttackerSpawner(List<MainTargetAttackerEntity.Factory> mainTargetAttackerFactory, MainTargetAttackerType type)
        {
            _mainTargetAttackerFactory = mainTargetAttackerFactory;
            _type = type;
        }

        public void Initialize(int capacityInPool)
        {
            foreach (var factory in _mainTargetAttackerFactory)
            {
                if (factory.Type == _type)
                    _mainTargetAttackerPool = new MonoObjectPooler<MainTargetAttackerEntity>(capacityInPool, "MainTargetAttackers", new InstantiateObjectsByFactory<MainTargetAttackerEntity>(factory));
            }
        }

        public Type GetTypeObject()
        {
            return typeof(MainTargetAttackerEntity);
        }

        public GameObject GetPrefab()
        {
            var prefab = _mainTargetAttackerPool.Get();
            _prefab = prefab.gameObject;
            _prefab.transform.position = new(30f,10f);
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
