using System;
using UnityEngine;

namespace Project.Content.CharacterAI
{
    [Serializable]
    public class DestroyerData : ICharacterData
    {
        [SerializeField] private DestroyerConfig _destroyerConfig;
        [SerializeField] private Transform _damageTextPoint;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Flags _flags;
        [SerializeField] private EntityFlags _enemyFlag;

        public float Speed => _destroyerConfig.Speed;
        public float Health => _destroyerConfig.Health;
        public float AttackCooldown => _destroyerConfig.AttackCooldown;
        public float SensorRadius => _destroyerConfig.SensorRadius;
        public float Damage => _destroyerConfig.Damage;
        public Transform CharacterTransform => _characterTransform;
        public Transform DamageTextPoint => _damageTextPoint;
        public Flags Flags => _flags;
        public EntityFlags EnemyFlag => _enemyFlag;
        public Vector2 HitColliderSize => _destroyerConfig.HitColliderSize;
        public Vector2 HitColliderOffset => _destroyerConfig.HitColliderOffset;
        public IEntity ThisEntity { get; set; }



    }
}

