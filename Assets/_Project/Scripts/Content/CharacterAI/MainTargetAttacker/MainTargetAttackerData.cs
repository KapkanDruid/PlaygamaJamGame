using Project.Content.BuildSystem;
using System;
using UnityEngine;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    [Serializable]
    class MainTargetAttackerData : ICharacterData, IAttackerData
    {
        [SerializeField] private MainTargetAttackerConfig _mainTargetAttackerConfig;
        [SerializeField] private Transform _damageTextPoint;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Flags _flags;
        [SerializeField] private EntityFlags[] _enemyFlag;
        [SerializeField] private FloatingTextConfig _floatingText;

        private SensorData _sensorData;

        public float Speed => _mainTargetAttackerConfig.Speed;
        public float Health => _mainTargetAttackerConfig.Health;
        public float Damage => _mainTargetAttackerConfig.Damage;
        public float SensorRadius => _mainTargetAttackerConfig.SensorRadius;
        public float AttackCooldown => _mainTargetAttackerConfig.AttackCooldown;
        public float HitColliderSize => _mainTargetAttackerConfig.HitColliderSize;
        public float DistanceToTarget => _mainTargetAttackerConfig.DistanceToTarget;
        public float ExperiencePoints => _mainTargetAttackerConfig.ExperiencePoints;
        public Vector2 HitColliderOffset => _mainTargetAttackerConfig.HitColliderOffset;
        public Transform CharacterTransform => _characterTransform;
        public Transform DamageTextPoint => _damageTextPoint;
        public Flags Flags => _flags;
        public IEntity ThisEntity { get; set; }
        public EntityFlags[] EnemyFlag => _enemyFlag;
        public MainTargetAttackerType Type => _mainTargetAttackerConfig.Type;
        public FloatingTextConfig FloatingText => _floatingText;
        public ISensorData SensorData => _sensorData;

        public void Initialize()
        {
            _sensorData = new SensorData();

            _sensorData.SensorOrigin = _characterTransform;
            _sensorData.SensorRadius = _mainTargetAttackerConfig.SensorRadius;
            _sensorData.ThisEntity = ThisEntity;
            _sensorData.TargetFlag = _enemyFlag;
        }
    }
}
