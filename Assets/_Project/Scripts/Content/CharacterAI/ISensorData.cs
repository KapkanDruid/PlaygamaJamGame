using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ISensorData
    {
        Transform CharacterTransform { get; }
        float SensorRadius { get; }
        EntityFlags EnemyFlag { get; }
        Flags Flags { get; }
        IEntity ThisEntity { get; set; }
        Vector2 HitColliderOffset { get; }
        float HitColliderSize { get; }

    }
}