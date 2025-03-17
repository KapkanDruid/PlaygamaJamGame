using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ICharacterData
    {
        float Speed { get; }
        float Damage { get; }
        float Health { get; }
        float AttackCooldown { get; }
        Transform DamageTextPoint { get; }
    }
}