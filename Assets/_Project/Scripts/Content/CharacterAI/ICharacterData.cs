using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ICharacterData
    {
        public float Speed { get; }
        public float Damage { get; }
        public float Health { get; }
        public float AttackCooldown { get; }
        public float DistanceToTarget { get; }
        public Transform DamageTextPoint { get; }
    }
}