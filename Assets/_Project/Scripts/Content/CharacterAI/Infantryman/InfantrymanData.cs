using Project.Content.BuildSystem;
using System;
using UnityEngine;

namespace Project.Content.CharacterAI.Infantryman
{
    [Serializable]
    public class InfantrymanData : IAllyEntityData, ITurretShootData, IProjectilePoolData
    {
        [SerializeField] InfantrymanConfig _infantrymanConfig;
        [SerializeField] private Transform _damageTextPoint;
        [SerializeField] private Transform _entityTransform;
        [SerializeField] private Transform _turretShootPoint;
        [SerializeField] private float _projectileLifeTime;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private Flags _flags;
        [SerializeField] private EntityFlags _enemyFlag;
        [SerializeField] private DirectProjectile _projectilePrefab;

        private DirectProjectileData _projectileData;
        private SensorData _sensorData;

        public float Speed => _infantrymanConfig.Speed;
        public float Health => _infantrymanConfig.Health;
        public float Damage => _infantrymanConfig.Damage;
        public float SensorRadius => _infantrymanConfig.SensorRadius;
        public float AttackCooldown => _infantrymanConfig.AttackCooldown;
        public float AttackRange => _infantrymanConfig.AttackRange;
        public Transform EntityTransform => _entityTransform;
        public Transform DamageTextPoint => _damageTextPoint;
        public Flags Flags => _flags;
        public IEntity ThisEntity { get; set; }
        public EntityFlags EnemyFlag => _enemyFlag;

        public float ReloadTime => _infantrymanConfig.AttackCooldown;
        public float RotateSpeed => _infantrymanConfig.Speed;
        public float RotationThreshold => _infantrymanConfig.Speed;
        public Transform ShootPoint => _turretShootPoint;
        public IDirectProjectileData ProjectileData => _projectileData;
        public DirectProjectile ProjectilePrefab => _projectilePrefab;
        public int ProjectilePoolCount => 10;
        public BuildSystem.ISensorData SensorData => _sensorData;
        public Transform RotationObject => throw new NotImplementedException();

        public void Initialize()
        {
            _projectileData = new DirectProjectileData();

            _projectileData.EnemyFlag = _enemyFlag;
            _projectileData.LifeTime = _projectileLifeTime;
            _projectileData.Damage = _infantrymanConfig.Damage;
            _projectileData.Speed = _projectileSpeed;

            _sensorData = new SensorData();

            _sensorData.SensorOrigin = _entityTransform;
            _sensorData.SensorRadius = _infantrymanConfig.SensorRadius;
            _sensorData.ThisEntity = ThisEntity;
            _sensorData.TargetFlag = _enemyFlag;
        }

    }
}

