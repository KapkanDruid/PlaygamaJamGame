using Project.Content.ReactiveProperty;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class TurretDynamicData
    {
        public readonly TurretType Type;

        private readonly TurretConfig _config;

        private float _maxHealth;
        private float _fireRate;

        private ReactiveProperty<float> _damage = new ReactiveProperty<float>();
        private ReactiveProperty<float> _sensorRadius = new ReactiveProperty<float>();

        public float MaxHealth => _maxHealth;
        public float FireRate => _fireRate;
        public IReactiveProperty<float> SensorRadius => _sensorRadius;
        public IReactiveProperty<float> Damage => _damage;
        public TurretConfig Config => _config;

        public TurretDynamicData(TurretType type, TurretConfig config)
        {
            Type = type;
            _config = config;

            _maxHealth = _config.MaxHealth;
            _fireRate = _config.FireRate;
            _sensorRadius.Value = _config.SensorRadius;
            _damage.Value = _config.ProjectileDamage;
        }

        public void IncreaseHealth(float value) => _maxHealth += Mathf.Abs(value);
        public void IncreaseFireRate(float value) => _fireRate += Mathf.Abs(value);
        public void IncreaseDamage(float value) => _damage.Value += Mathf.Abs(value);
        public void IncreaseSensorRadius(float value) => _sensorRadius.Value += Mathf.Abs(value);
    }
}