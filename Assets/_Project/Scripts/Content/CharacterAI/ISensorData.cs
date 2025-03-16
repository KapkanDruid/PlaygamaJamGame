using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ISensorData
    {
        Transform CharacterTransform { get; }
        float SensorRadius { get; }
        EntityFlags EnemyFlag { get; }
        IEntity ThisEntity { get; set; }

    }
}