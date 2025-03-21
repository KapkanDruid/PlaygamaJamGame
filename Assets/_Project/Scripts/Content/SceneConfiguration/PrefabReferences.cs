using Project.Content.BuildSystem;
using Project.Content.CharacterAI.Destroyer;
using Project.Content.CharacterAI.MainTargetAttacker;
using System;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class PrefabReferences
    {
        [Header("BuildingPrefabs")]
        [SerializeField] private MainBuildingEntity _mainBuildingFirstLevel;
        [SerializeField] private DestroyerHandler _destroyer;
        [SerializeField] private MainTargetAttackerHandler _mainTargetAttacker;

        //[Header("OtherPrefabs"), Space(3)]

        public MainBuildingEntity MainBuildingFirstLevel => _mainBuildingFirstLevel;
        public DestroyerHandler Destroyer => _destroyer;
        public MainTargetAttackerHandler MainTargetAttacker => _mainTargetAttacker;
    }
}