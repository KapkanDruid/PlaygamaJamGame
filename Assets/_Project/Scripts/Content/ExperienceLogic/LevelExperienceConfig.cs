using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content
{
    [CreateAssetMenu(fileName = "LevelExperienceConfig", menuName = "_Project/Config/LevelExperienceConfig")]
    public class LevelExperienceConfig : ScriptableObject
    {
        [SerializeField] private List<ExperienceData> _experienceData;
        public List<ExperienceData> LevelExperienceData => _experienceData;

        [Serializable]
        public class ExperienceData
        {
            [SerializeField] private int _level;
            [SerializeField] private float _pointsToReach;

            public int Level => _level;
            public float PointsToReach => _pointsToReach;
        }
    }
}
