using Project.Content.CharacterAI;
using Project.Content.ReactiveProperty;
using System;
using UnityEngine;


namespace Project.Content.BuildSystem
{
    [Serializable]
    public class BarracksData : IPlaceComponentData, IHealthData, IAllyEntityBarracks, IPLaceEffectData
    {
        [SerializeField] private Transform _gridPivotTransform;
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [SerializeField] private Transform[] _scalableObjects;
        [SerializeField] private GridPatternData _gridPattern;
        [SerializeField] private Flags _flags;
        [SerializeField] private GameObject[] _physicObjects;
        [SerializeField] private AllyEntityType _allyEntityType;
        [SerializeField] private BarracksType _barracksType;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private EffectType _placeSoundEffect;

        private BarrackDynamicData _dynamicData;
        private BarrackConfig _config;

        public Transform PivotTransform => _gridPivotTransform;
        public SpriteRenderer[] SpriteRenderers => _spriteRenderers;
        public Transform[] ScalableObjects => _scalableObjects;
        public GridPatternData GridPattern => _gridPattern;
        public GameObject[] PhysicObjects => _physicObjects;
        public Flags Flags => _flags;
        public AllyEntityType AllyEntityType => _allyEntityType;
        public BarracksType BarracksType => _barracksType;
        public Vector3 SpawnPosition => _spawnPosition.position;
        public int Capacity => _config.Capacity;
        public float SpawnCooldown => _config.SpawnCooldown;
        public float UnitDamageModifier => _dynamicData.UnitDamageModifier.Value;
        public float UnitHealthModifier => _dynamicData.UnitHealthModifier.Value;
        public int UnitUpgradeCount => _dynamicData.UnitUpgradeCount;
        public EffectType PlaceSoundEffect => _placeSoundEffect;

        public IReactiveProperty<float> Health => _dynamicData.BuildingMaxHealth;

        public void Construct(BarrackDynamicData dynamicData)
        {
            _dynamicData = dynamicData;
            _config = _dynamicData.Config;
        }
    }
}