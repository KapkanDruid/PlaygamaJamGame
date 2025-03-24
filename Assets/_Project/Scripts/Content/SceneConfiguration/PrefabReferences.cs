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
        [SerializeField] private TurretEntity _voiceOfTruthTurret;

        [Header("CharacterPrefabs"), Space(3)]
        [SerializeField] private DestroyerHandler _simpleParanoid;
        [SerializeField] private DestroyerHandler _advencedParanoid;
        [SerializeField] private DestroyerHandler _aliens;
        [SerializeField] private DestroyerHandler _flatEarther;
        [SerializeField] private MainTargetAttackerHandler _bigfoot;
        [SerializeField] private MainTargetAttackerHandler _humanMoth;

        [Header("OtherPrefabs"), Space(3)]
        [SerializeField] private GameObject _experienceObject;

        public MainBuildingEntity MainBuildingFirstLevel => _mainBuildingFirstLevel;
        public DestroyerHandler SimpleParanoid => _simpleParanoid;
        public DestroyerHandler AdvencedParanoid => _advencedParanoid;
        public DestroyerHandler Aliens => _aliens;
        public DestroyerHandler FlatEarther => _flatEarther;
        public MainTargetAttackerHandler BigFoot => _bigfoot;
        public MainTargetAttackerHandler HumanMoth => _humanMoth;
        public TurretEntity VoiceOfTruthTurret => _voiceOfTruthTurret;
        public GameObject ExperienceObject => _experienceObject; 
    }
}