using UnityEngine;


namespace Project.Content.BuildSystem
{
    [CreateAssetMenu(fileName = "BarrackConfig", menuName = "_Project/Config/BarrackConfig")]
    public class BarrackConfig : ScriptableObject
    {
        [Header("Static values")]
        [SerializeField] private BarracksType _type;
        [SerializeField] private int _capacity;
        [SerializeField] private float _spawnCooldown;

        [Header("Runtime-modified values")]
        [SerializeField] private float _buildingMaxHealth;

        private float _unitDamageModifier;
        private float _unitHealthModifier;

        public int Capacity => _capacity;
        public float SpawnCooldown => _spawnCooldown;
        public float UnitDamageModifier => _unitDamageModifier;
        public float UnitHealthModifier => _unitHealthModifier;
        public BarracksType Type => _type;
        public float BuildingMaxHealth => _buildingMaxHealth;
    }
}