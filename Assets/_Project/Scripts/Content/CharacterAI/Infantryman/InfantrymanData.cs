using Project.Content.BuildSystem;
using System;
using UnityEngine;

namespace Project.Content.CharacterAI.Infantryman
{
    [Serializable]
    public class InfantrymanData : IAllyEntityData, IShooterData, IProjectilePoolData
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
        private float _health;
        private int _levelUpgrade;

        public int LevelUpgrade => _levelUpgrade;
        public float Speed => _infantrymanConfig.Speed;
        public float Health => _health;
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
        public Transform ShootPoint => _turretShootPoint;
        public IDirectProjectileData ProjectileData => _projectileData;
        public DirectProjectile ProjectilePrefab => _projectilePrefab;
        public int ProjectilePoolCount => 10;
        public BuildSystem.ISensorData SensorData => _sensorData;
        public AllyEntityType Type => _infantrymanConfig.Type;

        public Sprite UpgradeSprite => _infantrymanConfig.UpgradeSprite;

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

            _health = _infantrymanConfig.Health;
        }

        public void UpdateData(InfantrymanSpawnData spawnData)
        {
            _projectileData.Damage = _infantrymanConfig.Damage;
            _projectileData.Damage += spawnData.DamageModifier;

            _health = _infantrymanConfig.Health;
            _health += spawnData.HealthModifier;

            _levelUpgrade = spawnData.LevelUpgradeModifier;
        }

        public Color GetColorForLevel(int level)
        {
            switch (level)
            {
                case 0: return new Color(0, 0, 0, 0);
                case 1: return Color.green;
                case 2: return Color.yellow;
                case 3: return new Color(1.0f, 0.5f, 0.0f); // Orange
                case 4: return Color.red;
                case 5: return new Color(0.5f, 0.0f, 0.5f); // Purple
                case 6: return Color.blue;
                case 7: return Color.cyan;
                case 8: return Color.magenta;
                case 9: return Color.white;
                default:
                    float gradient = Mathf.Clamp01((level - 3) / 10f);
                    return Color.Lerp(Color.red, Color.magenta, gradient);
            }
        }
    }

    public class InfantrymanSpawnData
    {
        private float _damageModifier;
        private float _healthModifier;
        private int _levelUpgradeModifier;
        public float DamageModifier { get => _damageModifier; set => _damageModifier = value; }
        public float HealthModifier { get => _healthModifier; set => _healthModifier = value; }
        public int LevelUpgradeModifier { get => _levelUpgradeModifier; set => _levelUpgradeModifier = value; }

        public InfantrymanSpawnData(float damageModifier, float healthModifier, int levelUpgradeModifier = 0)
        {
            _damageModifier = damageModifier;
            _healthModifier = healthModifier;
            _levelUpgradeModifier = levelUpgradeModifier;
        }
    }
}

