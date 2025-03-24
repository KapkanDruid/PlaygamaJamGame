using Project.Content.BuildSystem;
using System;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class ConfigReferences
    {
        [SerializeField] private TurretConfig[] _turretConfigs;
        [SerializeField] private LevelExperienceConfig _levelExperienceConfig;
        public TurretConfig[] TurretConfigs => _turretConfigs;
        public LevelExperienceConfig LevelExperienceConfig => _levelExperienceConfig; 
    }
}