using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ICharacterData
    {
        float AttackCooldown { get; }
        float Damage { get; }
        Transform DamageTextPoint { get; }
        Flags Flags { get; }
        float Health { get; }
        Vector2 HitColliderOffset { get; }
        Vector2 HitColliderSize { get; }
        float Speed { get; }
    }
}