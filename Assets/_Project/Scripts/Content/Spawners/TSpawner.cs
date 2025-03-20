using Project.Content.CharacterAI;
using UnityEngine;

namespace Project.Content.Spawners
{
    public class TSpawner<T> : ISpawner where T : MonoBehaviour
    {
        private readonly ObjectPooler<T> _objectPool;

        public TSpawner(T prefab, int poolSize, string parentName)
        {
            _objectPool = new ObjectPooler<T>(prefab, poolSize, parentName);
        }

        public void Spawn(GameObject prefab, ICharacterData data, int id, TypeAlly typeAlly, Transform spawnPosition)
        {
            Prepare();

        }

        public void Spawn(GameObject prefab, Transform spawnPosition)
        {
            var obj = _objectPool.Get();
            obj.transform.position = spawnPosition.position;
        }


        private void Prepare()
        {

        }
    }
}