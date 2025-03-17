using System;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    [Serializable]
    public class MainBuildingData : IPlaceComponentData
    {
        [SerializeField] private Transform _gridPivotTransform;
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [SerializeField] private Transform[] _scalableObjects;
        [SerializeField] private GridPatternData _gridPattern;

        public Transform PivotTransform => _gridPivotTransform;
        public SpriteRenderer[] SpriteRenderers => _spriteRenderers;
        public Transform[] ScalableObjects => _scalableObjects;
        public GridPatternData GridPattern => _gridPattern;
    }
}
