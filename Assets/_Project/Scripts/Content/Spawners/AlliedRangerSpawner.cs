using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Infantryman;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.Spawners
{
    public class AlliedRangerSpawner : IEnemySpawner
    {
        private ObjectPooler<InfantrymanEntity> _barracksPool;
        private List<InfantrymanEntity.Factory> _barracksFactory;
        private AllyEntityType _type;
        private GameObject _prefab;
        public GameObject Prefab => _prefab;

        public class Factory : PlaceholderFactory<AllyEntityType, AlliedRangerSpawner> { }

        public AlliedRangerSpawner(List<InfantrymanEntity.Factory> allyFactory, AllyEntityType type)
        {
            _barracksFactory = allyFactory;
            _type = type;
        }

        public void Initialize(int capacityInPool)
        {
            foreach (var factory in _barracksFactory)
            {
                if (factory.Type == _type)
                    _barracksPool = new ObjectPooler<InfantrymanEntity>(capacityInPool, "Allies", new InstantiateObjectsByFactory<InfantrymanEntity>(factory));
            }
        }

        public Type GetTypeObject()
        {
            return typeof(InfantrymanEntity);
        }

        public void Spawn(Vector3 position)
        {
            var prefab = _barracksPool.Get();
            prefab.transform.position = position;

            _prefab = prefab.gameObject;
        }

    }
}
