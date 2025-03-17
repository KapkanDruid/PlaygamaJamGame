using System;
using UnityEngine;

namespace Project.Content.CharacterAI.Destroyer
{
    [Serializable]
    public class DestroyerData : ICharacterData, ISensorData
    {
        [SerializeField] private DestroyerConfig _destroyerConfig;
        [SerializeField] private Transform _damageTextPoint;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Flags _flags;
        [SerializeField] private EntityFlags _enemyFlag;

        public float Speed => _destroyerConfig.Speed;
        public float Health => _destroyerConfig.Health;
        public float Damage => _destroyerConfig.Damage;
        public float SensorRadius => _destroyerConfig.SensorRadius;
        public float AttackCooldown => _destroyerConfig.AttackCooldown;
        public float HitColliderSize => _destroyerConfig.HitColliderSize;
        public Vector2 HitColliderOffset => _destroyerConfig.HitColliderOffset;
        public Transform CharacterTransform => _characterTransform;
        public Transform DamageTextPoint => _damageTextPoint;
        public Flags Flags => _flags;
        public IEntity ThisEntity { get; set; }
        public EntityFlags EnemyFlag => _enemyFlag;



    }
}

