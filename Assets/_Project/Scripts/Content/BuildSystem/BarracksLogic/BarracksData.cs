using Project.Content.CharacterAI;
using Project.Content.ReactiveProperty;
using System;
using UnityEngine;


namespace Project.Content.BuildSystem
{
    [Serializable]
    public class BarracksData : IPlaceComponentData, IHealthData, IAllyEntityBarracks
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private Transform _gridPivotTransform;
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [SerializeField] private Transform[] _scalableObjects;
        [SerializeField] private GridPatternData _gridPattern;
        [SerializeField] private Flags _flags;
        [SerializeField] private GameObject[] _physicObjects;
        [SerializeField] private AllyEntityType _allyEntityType;
        [SerializeField] private BarracksType _barracksType;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private int _capacity;
        [SerializeField] private float _spawnCooldown;

        private ReactiveProperty<float> _health;

        //public float Health => _maxHealth;
        public Transform PivotTransform => _gridPivotTransform;
        public SpriteRenderer[] SpriteRenderers => _spriteRenderers;
        public Transform[] ScalableObjects => _scalableObjects;
        public GridPatternData GridPattern => _gridPattern;
        public GameObject[] PhysicObjects => _physicObjects;
        public Flags Flags => _flags;
        public AllyEntityType AllyEntityType => _allyEntityType;
        public BarracksType BarracksType => _barracksType;
        public Vector3 SpawnPosition => _spawnPosition.position;
        public int Capacity => _capacity;
        public float SpawnCooldown => _spawnCooldown;

        IReactiveProperty<float> IHealthData.Health => _health;

        public void Initialize()
        {
            _health = new ReactiveProperty<float>(_maxHealth);
        }
    }
}