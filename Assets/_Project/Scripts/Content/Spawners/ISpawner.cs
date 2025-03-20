using Project.Content.CharacterAI;
using UnityEngine;

namespace Project.Content.Spawners
{
    public interface ISpawner
    {
        public void Spawn(Transform spawnPosition);
        public void Spawn(ICharacterData data, int id, TypeAlly typeAlly, Transform spawnPosition);
    }
}