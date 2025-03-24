using Project.Content.CharacterAI;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public interface IAllyEntityBarracks
    {
        public AllyEntityType AllyEntityType { get; }
        public Vector3 SpawnPosition { get; }
        public int Capacity { get; }
        public float SpawnCooldown { get; }

    }
}
