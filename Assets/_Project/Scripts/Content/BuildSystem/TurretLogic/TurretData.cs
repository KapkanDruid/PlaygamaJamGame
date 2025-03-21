using System;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    [Serializable]
    public class TurretData : IPlaceComponentData, IHealthData, ITurretShootData, IProjectilePoolData
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _projectileLifeTime;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileDamage;
        [SerializeField] private float _sensorRadius;
        [SerializeField] private float _rotationThreshold;
        [SerializeField] private DirectProjectile _projectilePrefab;

        [SerializeField] private Transform _gridPivotTransform;
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [SerializeField] private Transform[] _scalableObjects;
        [SerializeField] private GridPatternData _gridPattern;
        [SerializeField] private Flags _flags;
        [SerializeField] private GameObject[] _physicObjects;
        [SerializeField] private Transform _turretRotationObject;
        [SerializeField] private Transform _turretShootPoint;

        [SerializeField] private EntityFlags _enemyFlag;

        private DirectProjectileData _projectileData;
        private SensorData _sensorData;

        [Inject] private IEntity _thisEntity;

        public float Health => _maxHealth;
        public float FireRate => _fireRate;
        public int ProjectilePoolCount => 10;

        public Transform PivotTransform => _gridPivotTransform;
        public SpriteRenderer[] SpriteRenderers => _spriteRenderers;
        public Transform[] ScalableObjects => _scalableObjects;
        public GridPatternData GridPattern => _gridPattern;
        public Flags Flags => _flags;
        public GameObject[] PhysicObjects => _physicObjects;
        public Transform RotationObject => _turretRotationObject;
        public Transform ShootPoint => _turretShootPoint;
        public DirectProjectile ProjectilePrefab => _projectilePrefab;
 
        public IDirectProjectileData ProjectileData => _projectileData;
        public ISensorData SensorData => _sensorData;
        public IEntity ThisEntity { get => _thisEntity; set => _thisEntity = value; }
        public float RotateSpeed => _rotateSpeed;
        public float RotationThreshold => _rotationThreshold;

        public void Initialize()
        {
            _projectileData = new DirectProjectileData();

            _projectileData.EnemyFlag = _enemyFlag;
            _projectileData.LifeTime = _projectileLifeTime;
            _projectileData.Damage = _projectileDamage;
            _projectileData.Speed = _projectileSpeed;

            _sensorData = new SensorData();

            _sensorData.SensorOrigin = _turretRotationObject;
            _sensorData.SensorRadius = _sensorRadius;
            _sensorData.ThisEntity = ThisEntity;
            _sensorData.TargetFlag = _enemyFlag;
        }
    }
}