using System;
using UnityEngine;

namespace Project.Content.Spawners
{
    public interface IEnemySpawner
    {
        public void Initialize();
        public Type GetTypeObject();
        public void Spawn(Vector3 position);
    }
}
