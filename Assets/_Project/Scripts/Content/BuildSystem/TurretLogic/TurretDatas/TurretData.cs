using Project.Content.ReactiveProperty;
using System;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    [Serializable]
    public class TurretData : IPlaceComponentData, IHealthData, ITurretShootData, IProjectilePoolData, IDisposable
    {
        [SerializeField] private int _projectilePoolSize;
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
        [SerializeField] private TurretType _turretType;

        private DirectProjectileData _projectileData = new DirectProjectileData();
        private SensorData _sensorData = new SensorData();
        private TurretDynamicData _dynamicData;
        private TurretConfig _configData;
        private ReactiveProperty<float> _health;

        private IEntity _thisEntity;

        public float ReloadTime => _dynamicData.ReloadTime;
        public int ProjectilePoolCount => _projectilePoolSize;

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
        public IEntity ThisEntity => _thisEntity;
        public float RotateSpeed => _configData.RotateSpeed;
        public float RotationThreshold => _configData.RotationThreshold;

        public TurretType TurretType => _turretType;

        public IReactiveProperty<float> Health => _health;

        public void Construct(TurretDynamicData dynamicData, IEntity entity)
        {
            _dynamicData = dynamicData;
            _thisEntity = entity;

            if (dynamicData.Type != _turretType)
                Debug.LogError("Wrong type of data received by " + ToString());
        }

        public void Initialize()
        {
            _configData = _dynamicData.Config;

            _projectileData.EnemyFlag = _enemyFlag;
            _projectileData.LifeTime = _configData.ProjectileLifeTime;
            _projectileData.Speed = _configData.ProjectileSpeed;

            _sensorData.SensorOrigin = _turretRotationObject;
            _sensorData.ThisEntity = ThisEntity;
            _sensorData.TargetFlag = _enemyFlag;

            _sensorData.SensorRadius = _dynamicData.SensorRadius.Value;
            _projectileData.Damage = _dynamicData.Damage.Value;

            _health = new ReactiveProperty<float>(_dynamicData.MaxHealth.Value);

            _dynamicData.MaxHealth.OnValueChanged += UpdateHealth;
            _dynamicData.SensorRadius.OnValueChanged += UpdateSensorRadius;
            _dynamicData.Damage.OnValueChanged += UpdateDamage;
        }

        private void UpdateHealth(float value) => _health.Value = value;
        private void UpdateSensorRadius(float value) => _sensorData.SensorRadius = value;
        private void UpdateDamage(float value) => _projectileData.Damage = value;

        public void Dispose()
        {
            _dynamicData.MaxHealth.OnValueChanged -= UpdateHealth;
            _dynamicData.SensorRadius.OnValueChanged -= UpdateSensorRadius;
            _dynamicData.Damage.OnValueChanged -= UpdateDamage;
        }
    }
}