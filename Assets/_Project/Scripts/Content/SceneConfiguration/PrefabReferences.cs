using Project.Content.BuildSystem;
using System;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class PrefabReferences
    {
        [Header("BuildingPrefabs")]
        [SerializeField] private MainBuildingEntity _mainBuildingFirstLevel;

        [Header("OtherPrefabs"), Space(3)]
        [SerializeField] private Canvas _hpBarCanvas;

        public MainBuildingEntity MainBuildingFirstLevel => _mainBuildingFirstLevel;
        public Canvas HpBarCanvas => _hpBarCanvas; 
    }
}