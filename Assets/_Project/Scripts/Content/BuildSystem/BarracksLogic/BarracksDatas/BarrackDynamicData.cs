using Project.Content.ReactiveProperty;
using System;

namespace Project.Content.BuildSystem
{
    public class BarrackDynamicData
    {
        public readonly BarracksType Type;
        private readonly BarrackConfig _config;
        public event Action OnDataUpdate;

        private ReactiveProperty<float> _unitDamageModifier = new ReactiveProperty<float>();
        private ReactiveProperty<float> _unitHealthModifier = new ReactiveProperty<float>();
        private ReactiveProperty<float> _buildingMaxHealth = new ReactiveProperty<float>();

        public ReactiveProperty<float> BuildingMaxHealth => _buildingMaxHealth;
        public BarrackConfig Config => _config;
        public ReactiveProperty<float> UnitDamageModifier => _unitDamageModifier; 
        public ReactiveProperty<float> UnitHealthModifier => _unitHealthModifier;

        public BarrackDynamicData(BarrackConfig config)
        {
            _buildingMaxHealth.SetPredicate(value => value >= 0);

            _config = config;

            Type = config.Type;

            _unitDamageModifier.Value = config.UnitDamageModifier;
            _unitHealthModifier.Value = config.UnitHealthModifier;
            _buildingMaxHealth.Value = config.BuildingMaxHealth;

            _unitDamageModifier.OnValueChanged += (x) => OnDataUpdate?.Invoke();
            _unitHealthModifier.OnValueChanged += (x) => OnDataUpdate?.Invoke();
            _buildingMaxHealth.OnValueChanged += (x) => OnDataUpdate?.Invoke();
        }
    }
}