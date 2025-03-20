using Project.Content.CharacterAI;
using UnityEngine;

namespace Project.Content.Spawners
{
    public interface ISpawner
    {
        public void Spawn(GameObject prefab, Transform spawnPosition);
        public void Spawn(GameObject prefab, ICharacterData data, int id, TypeAlly typeAlly, Transform spawnPosition);
    }
}