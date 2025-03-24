using Project.Content.BuildSystem;
using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Destroyer;
using Project.Content.CharacterAI.Infantryman;
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
        [SerializeField] private TurretEntity _voiceOfTruthTurret;
        [SerializeField] private BarracksEntity _infantrymanBarracks;
        [SerializeField] private DestroyerHandler _simpleParanoid;
        [SerializeField] private DestroyerHandler _advencedParanoid;
        [SerializeField] private DestroyerHandler _aliens;
        [SerializeField] private DestroyerHandler _flatEarther;
        [SerializeField] private MainTargetAttackerHandler _bigfoot;
        [SerializeField] private MainTargetAttackerHandler _humanMoth;
        [SerializeField] private InfantrymanEntity _infantryman;
        [SerializeField] private DefensiveFlag _flag;

        //[Header("OtherPrefabs"), Space(3)]

        public MainBuildingEntity MainBuildingFirstLevel => _mainBuildingFirstLevel;
        public DestroyerHandler SimpleParanoid => _simpleParanoid;
        public DestroyerHandler AdvencedParanoid => _advencedParanoid;
        public DestroyerHandler Aliens => _aliens;
        public DestroyerHandler FlatEarther => _flatEarther;
        public MainTargetAttackerHandler BigFoot => _bigfoot;
        public MainTargetAttackerHandler HumanMoth => _humanMoth;
        public InfantrymanEntity Infantryman => _infantryman;
        public TurretEntity VoiceOfTruthTurret => _voiceOfTruthTurret; 
        public BarracksEntity InfantrymanBarracks => _infantrymanBarracks; 
        public DefensiveFlag Flag => _flag;
    }
}