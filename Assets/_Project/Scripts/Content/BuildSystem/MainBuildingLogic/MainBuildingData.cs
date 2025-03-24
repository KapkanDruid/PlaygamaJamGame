using Project.Content.ReactiveProperty;
using System;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    [Serializable]
    public class MainBuildingData : IPlaceComponentData, IHealthData
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private Transform _gridPivotTransform;
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [SerializeField] private Transform[] _scalableObjects;
        [SerializeField] private GridPatternData _gridPattern;
        [SerializeField] private Flags _flags;
        [SerializeField] private GameObject[] _physicObjects;

        private ReactiveProperty<float> _health = new ReactiveProperty<float>();

        public Transform PivotTransform => _gridPivotTransform;
        public SpriteRenderer[] SpriteRenderers => _spriteRenderers;
        public Transform[] ScalableObjects => _scalableObjects;
        public GridPatternData GridPattern => _gridPattern;
        public Flags Flags => _flags;
        public GameObject[] PhysicObjects => _physicObjects;

        public IReactiveProperty<float> Health => _health;

        public void Initialize()
        {
            _health.Value = _maxHealth;
        }
    }
}
