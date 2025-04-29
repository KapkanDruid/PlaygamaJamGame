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
        [SerializeField] private DestroyerEntity _simpleParanoid;
        [SerializeField] private DestroyerEntity _advencedParanoid;
        [SerializeField] private DestroyerEntity _aliens;
        [SerializeField] private DestroyerEntity _flatEarther;
        [SerializeField] private MainTargetAttackerEntity _bigfoot;
        [SerializeField] private MainTargetAttackerEntity _humanMoth;
        [SerializeField] private InfantrymanEntity _infantryman;
        
        [Header("OtherPrefabs"), Space(3)]
        [SerializeField] private GameObject _experienceObject;
        [SerializeField] private GameObject _upgradeObject;
        [SerializeField] private DefensiveFlag _flag;
        [SerializeField] private FloatingText[] _floatingTextPrefabs;
        [SerializeField] private List<SimpleProjectile> _simpleProjectilePrefabs;

        public MainBuildingEntity MainBuildingFirstLevel => _mainBuildingFirstLevel;
        public DestroyerEntity SimpleParanoid => _simpleParanoid;
        public DestroyerEntity AdvencedParanoid => _advencedParanoid;
        public DestroyerEntity Aliens => _aliens;
        public DestroyerEntity FlatEarther => _flatEarther;
        public MainTargetAttackerEntity BigFoot => _bigfoot;
        public MainTargetAttackerEntity HumanMoth => _humanMoth;
        public InfantrymanEntity Infantryman => _infantryman;
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