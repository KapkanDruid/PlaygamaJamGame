﻿using Project.Content.ReactiveProperty;
using System;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    [Serializable]
    public class WallData : IPlaceComponentData, IHealthData
    {
        [SerializeField] private Transform _gridPivotTransform;
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [SerializeField] private Transform[] _scalableObjects;
        [SerializeField] private GridPatternData _gridPattern;
        [SerializeField] private Flags _flags;
        [SerializeField] private GameObject[] _physicObjects;

        private WallConfig _config;
        private WallDynamicData _dynamicData;

        public IReactiveProperty<float> Health => _dynamicData.BuildingMaxHealth;
        public Transform PivotTransform => _gridPivotTransform;
        public SpriteRenderer[] SpriteRenderers => _spriteRenderers;
        public Transform[] ScalableObjects => _scalableObjects;
        public GridPatternData GridPattern => _gridPattern;
        public GameObject[] PhysicObjects => _physicObjects;
        public Flags Flags => _flags; 

        public void Construct(WallDynamicData dynamicData)
        {
            _dynamicData = dynamicData;
            _config = _dynamicData.Config;
        }
    }
}
