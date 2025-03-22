using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Destroyer;
using UnityEngine;
using Zenject;

namespace Project.Content.Spawners
{
    public class Spawner<T> : ISpawner where T : MonoBehaviour
    {
        private readonly ObjectPooler<T> _objectPool;

        public Spawner(T prefab, int poolSize, string parentName)
        {
        }

        public void Spawn(ICharacterData data, int id, TypeUnitAlly typeAlly, Transform spawnPosition)
        {
            Prepare();

        }

        public void Spawn(Transform spawnPosition)
        {
            var obj = _objectPool.Get();
            obj.transform.position = spawnPosition.position;
        }


        private void Prepare()
        {

        }
    }
}