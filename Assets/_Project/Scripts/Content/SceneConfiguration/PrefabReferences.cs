using Project.Content.BuildSystem;
using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Destroyer;
using Project.Content.CharacterAI.Infantryman;
using Project.Content.CharacterAI.MainTargetAttacker;
using Project.Content.ProjectileSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class PrefabReferences
    {
        [Header("BuildingPrefabs")]
        [SerializeField] private MainBuildingEntity _mainBuildingFirstLevel;
        [SerializeField] private TurretEntity _voiceOfTruthTurret;
        [SerializeField] private BarracksEntity _infantrymanBarracks;
        [SerializeField] private WallEntity _wallPrefab;

        [Header("CharacterPrefabs"), Space(3)]
        [SerializeField] private List<DestroyerEntity> _destroyers;
        [SerializeField] private List<MainTargetAttackerEntity> _mainTargetAttackers;
        [SerializeField] private List<InfantrymanEntity> _infantrymen;
        
        [Header("OtherPrefabs"), Space(3)]
        [SerializeField] private GameObject _experienceObject;
        [SerializeField] private GameObject _upgradeObject;
        [SerializeField] private DefensiveFlag _flag;
        [SerializeField] private FloatingText[] _floatingTextPrefabs;
        [SerializeField] private List<SimpleProjectile> _simpleProjectilePrefabs;

        public MainBuildingEntity MainBuildingFirstLevel => _mainBuildingFirstLevel;
        public List<DestroyerEntity> DestroyersPrefabs => _destroyers;
        public List<MainTargetAttackerEntity> MainTargetAttackersPrefabs => _mainTargetAttackers;
        public List<InfantrymanEntity> Infantrymen => _infantrymen;
        public TurretEntity VoiceOfTruthTurret => _voiceOfTruthTurret; 
        public GameObject ExperienceObject => _experienceObject; 
        public BarracksEntity InfantrymanBarracks => _infantrymanBarracks; 
        public DefensiveFlag Flag => _flag;
        public FloatingText[] FloatingTextPrefabs => _floatingTextPrefabs;
        public WallEntity WallPrefab => _wallPrefab;
        public GameObject UpgradeObject => _upgradeObject;
        public List<SimpleProjectile> SimpleProjectilePrefabs => _simpleProjectilePrefabs;
    }
}