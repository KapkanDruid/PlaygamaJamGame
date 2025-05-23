using Project.Content.BuildSystem;
using System;
using UnityEngine;

namespace Project.Content.CharacterAI.Destroyer
{
    [Serializable]
    public class DestroyerData : ICharacterData, IAttackerData
    {
        [SerializeField] private DestroyerConfig _destroyerConfig;
        [SerializeField] private Transform _damageTextPoint;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Flags _flags;
        [SerializeField] private EntityFlags[] _enemyFlag;
        [SerializeField] private FloatingTextConfig _floatingText;
        [SerializeField] private Collider2D _collider;

        private SensorData _sensorData;

        public float Speed => _destroyerConfig.Speed;
        public float Health => _destroyerConfig.Health;
        public float Damage => _destroyerConfig.Damage;
        public float SensorRadius => _destroyerConfig.SensorRadius;
        public float AttackCooldown => _destroyerConfig.AttackCooldown;
        public float HitColliderSize => _destroyerConfig.HitColliderSize;
        public float DistanceToTarget => _destroyerConfig.DistanceToTarget;
        public float ExperiencePoints => _destroyerConfig.ExperiencePoints;
        public Vector2 HitColliderOffset => _destroyerConfig.HitColliderOffset;
        public Transform CharacterTransform => _characterTransform;
        public Transform DamageTextPoint => _damageTextPoint;
        public Flags Flags => _flags;
        public IEntity ThisEntity { get; set; }
        public EntityFlags[] EnemyFlag => _enemyFlag;
        public DestroyerType Type => _destroyerConfig.Type;
        public FloatingTextConfig FloatingText => _floatingText;
        public ISensorData SensorData => _sensorData;
        public Collider2D Collider => _collider;

        public void Initialize()
        {
            _sensorData = new SensorData();

            _sensorData.SensorOrigin = _characterTransform;
            _sensorData.SensorRadius = _destroyerConfig.SensorRadius;
            _sensorData.ThisEntity = ThisEntity;
            _sensorData.TargetFlag = _enemyFlag;
        }
    }
}

