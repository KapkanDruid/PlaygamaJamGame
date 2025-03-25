using Project.Content.ReactiveProperty;


namespace Project.Content.BuildSystem
{
    public class BarrackDynamicData
    {
        public readonly BarracksType Type;
        private readonly BarrackConfig _config;

        private float _unitDamageModifier;
        private float _unitHealthModifier;
        private ReactiveProperty<float> _buildingMaxHealth = new ReactiveProperty<float>();

        public ReactiveProperty<float> BuildingMaxHealth => _buildingMaxHealth;
        public BarrackConfig Config => _config;
        public float UnitDamageModifier { get => _unitDamageModifier; set => _unitDamageModifier = value; }
        public float UnitHealthModifier { get => _unitHealthModifier; set => _unitHealthModifier = value; }

        public BarrackDynamicData(BarrackConfig config)
        {
            _buildingMaxHealth.SetPredicate(value => value >= 0);

            _config = config;

            Type = config.Type;

            _unitDamageModifier = config.UnitDamageModifier;
            _unitHealthModifier = config.UnitHealthModifier;
            _buildingMaxHealth.Value = config.BuildingMaxHealth;
        }
    }
}