using Project.Content.ReactiveProperty;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class TurretDynamicData
    {
        public readonly TurretType Type;

        private readonly TurretConfig _config;

        private float _maxHealth;
        private float _reloadTime;

        private ReactiveProperty<float> _damage = new ReactiveProperty<float>();
        private ReactiveProperty<float> _sensorRadius = new ReactiveProperty<float>();

        public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        public float ReloadTime
        {
            get => _reloadTime;
            set
            {
                if (_reloadTime == value)
                    return;

                if (value <= 0.2)
                    _reloadTime = 0.2f;
                else
                    _reloadTime = value;
            }
        }
        public ReactiveProperty<float> Damage { get => _damage; set => _damage = value; }
        public ReactiveProperty<float> SensorRadius { get => _sensorRadius; set => _sensorRadius = value; }
        public TurretConfig Config => _config;

        public TurretDynamicData(TurretConfig config)
        {
            _config = config;
            Type = _config.Type;

            MaxHealth = _config.MaxHealth;
            ReloadTime = _config.FireRate;
            SensorRadius.Value = _config.SensorRadius;
            Damage.Value = _config.ProjectileDamage;
        }
    }
}