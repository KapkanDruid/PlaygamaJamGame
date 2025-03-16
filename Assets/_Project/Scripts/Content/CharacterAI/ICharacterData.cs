using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface ICharacterData
    {
        float AttackCooldown { get; }
        Transform CharacterTransform { get; }
        float Damage { get; }
        Transform DamageTextPoint { get; }
        EntityFlags EnemyFlag { get; }
        Flags Flags { get; }
        float Health { get; }
        Vector2 HitColliderOffset { get; }
        Vector2 HitColliderSize { get; }
        float SensorRadius { get; }
        float Speed { get; }
        IEntity ThisEntity { get; set; }
    }
}