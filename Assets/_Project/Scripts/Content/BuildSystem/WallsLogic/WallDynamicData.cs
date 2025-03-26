using Project.Content.ReactiveProperty;
using System;

namespace Project.Content.BuildSystem
{
    public class WallDynamicData
    {
        private WallConfig _config;
        public event Action OnDataUpdate;

        private ReactiveProperty<float> _buildingMaxHealth = new ReactiveProperty<float>();
        public ReactiveProperty<float> BuildingMaxHealth => _buildingMaxHealth;
        public WallConfig Config  => _config;

        public WallDynamicData(WallConfig config)
        {
            _config = config;

            _buildingMaxHealth.Value = _config.MaxHealth;
            _buildingMaxHealth.OnValueChanged += (x) => OnDataUpdate?.Invoke();
        }
    }
}
