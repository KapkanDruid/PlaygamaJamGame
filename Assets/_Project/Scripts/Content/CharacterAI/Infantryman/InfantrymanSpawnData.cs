namespace Project.Content.CharacterAI.Infantryman
{
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

