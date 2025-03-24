using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ICharacterConfig
    {
        public int Damage { get; }
        public float Speed { get; }
        public float Health { get; }
        public float SensorRadius { get; }
        public float AttackCooldown { get; }
        public float HitColliderSize { get; }
        public float DistanceToTarget { get; }
        public Vector2 HitColliderOffset { get; }
    }
}