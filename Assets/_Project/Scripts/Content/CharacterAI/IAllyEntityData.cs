using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface IAllyEntityData
    {
        public float Speed { get; }
        public float Damage { get; }
        public float Health { get; }
        public float AttackCooldown { get; }
        public float AttackRange { get; }
        public Transform DamageTextPoint { get; }
    }
}