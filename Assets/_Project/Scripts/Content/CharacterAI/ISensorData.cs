using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ISensorData
    {
        public float SensorRadius { get; }
        public Flags Flags { get; }
        public Transform CharacterTransform { get; }
        public EntityFlags EnemyFlag { get; }
        public IEntity ThisEntity { get; }
        public Vector2 HitColliderOffset { get; }
        public float HitColliderSize { get;  }

    }
}