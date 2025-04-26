using Project.Content.CharacterAI;
using System;
using UnityEngine;

namespace Project.Content.Spawners
{
    public interface ISpawner
    {
        public void Initialize(int capacityInPool);
        public Type GetTypeObject();
        public void Spawn(Vector3 position);
        public GameObject GetPrefab();
    }
}
