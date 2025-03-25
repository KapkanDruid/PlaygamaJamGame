using Project.Content.CharacterAI;
using Project.Content.ReactiveProperty;
using System;
using UnityEngine;


namespace Project.Content.BuildSystem
{
    [Serializable]
    public class BarracksData : IPlaceComponentData, IHealthData, IAllyEntityBarracks
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
        public float UnitDamageModifier => _dynamicData.UnitDamageModifier;
        public float UnitHealthModifier => _dynamicData.UnitHealthModifier;

        public IReactiveProperty<float> Health => _dynamicData.BuildingMaxHealth;

        public void Construct(BarrackDynamicData dynamicData)
        {
            _dynamicData = dynamicData;
            _config = _dynamicData.Config;
        }
    }
}