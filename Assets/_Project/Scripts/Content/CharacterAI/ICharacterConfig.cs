using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ICharacterConfig
    {
        int Damage { get; }
        float AttackCooldown { get; }
        float Health { get; }
        float SensorRadius { get; }
        float Speed { get; }
        Vector2 HitColliderOffset { get; }
        Vector2 HitColliderSize { get; }
    }
}