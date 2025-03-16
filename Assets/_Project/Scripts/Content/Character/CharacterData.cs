using System;
using UnityEngine;

namespace Content.Character
{
    [Serializable]
    public class CharacterData
    {
        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private Transform _damageTextPoint;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Flags _flags;
        [SerializeField] private EntityFlags _enemyFlag;

        public float Speed => _characterConfig.Speed;
        public float Health => _characterConfig.Health;
        public float AttackCooldown => _characterConfig.AttackCooldown;
        public float SensorRadius => _characterConfig.SensorRadius;
        public float Damage => _characterConfig.Damage;
        public Transform CharacterTransform => _characterTransform;
        public Transform DamageTextPoint => _damageTextPoint;
        public Flags Flags => _flags;
        public EntityFlags EnemyFlag => _enemyFlag;
        public Vector2 HitColliderSize => _characterConfig.HitColliderSize;
        public Vector2 HitColliderOffset => _characterConfig.HitColliderOffset;
        public IEntity ThisEntity { get; set; }



    }
}

