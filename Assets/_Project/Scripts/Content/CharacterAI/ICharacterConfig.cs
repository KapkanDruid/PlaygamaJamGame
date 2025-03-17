using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ICharacterConfig
    {
        int Damage { get; }
        float Speed { get; }
        float Health { get; }
        float SensorRadius { get; }
        float AttackCooldown { get; }
        float HitColliderSize { get; }
        Vector2 HitColliderOffset { get; }
    }
}