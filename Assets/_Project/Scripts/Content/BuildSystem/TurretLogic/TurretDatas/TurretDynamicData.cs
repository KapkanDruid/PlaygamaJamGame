using Project.Content.ReactiveProperty;
using System;

namespace Project.Content.BuildSystem
{
    public class TurretDynamicData
    {
        public readonly TurretType Type;
        private readonly TurretConfig _config;
        private float _reloadTime;

        public event Action OnDataUpdate;

        private ReactiveProperty<float> _maxHealth = new ReactiveProperty<float>();
        private ReactiveProperty<float> _damage = new ReactiveProperty<float>();
        private ReactiveProperty<float> _sensorRadius = new ReactiveProperty<float>();

        public float ReloadTime
        {
            get => _reloadTime;
            set
            {
                if (_reloadTime == value)
                    return;

                if (value <= 0.2)
                {
                    _reloadTime = 0.2f;
                    OnDataUpdate?.Invoke();
                }
                else
                {
                    _reloadTime = value;

                    OnDataUpdate?.Invoke();
                }
            }
        }
        public ReactiveProperty<float> MaxHealth => _maxHealth;
        public ReactiveProperty<float> Damage => _damage; 
        public ReactiveProperty<float> SensorRadius => _sensorRadius;

        public TurretConfig Config => _config;

        public TurretDynamicData(TurretConfig config)
        {
            _maxHealth.SetPredicate(value => value >= 0);

            _config = config;
            Type = _config.Type;

            _maxHealth.Value = _config.MaxHealth;
            _reloadTime = _config.FireRate;
            _sensorRadius.Value = _config.SensorRadius;
            _damage.Value = _config.ProjectileDamage;

            _maxHealth.OnValueChanged += (value) => OnDataUpdate?.Invoke();
            _damage.OnValueChanged += (value) => OnDataUpdate?.Invoke();
            _sensorRadius.OnValueChanged += (value) => OnDataUpdate?.Invoke();
        }
    }
}