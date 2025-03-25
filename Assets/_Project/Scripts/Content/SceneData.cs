using Project.Content.BuildSystem;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField] private Vector2Int _groundGridSize;
        [SerializeField] private Vector2 _startFlagPosition;
        [Header("Time in Seconds")]
        [SerializeField] private float _timeToWin;

        private Dictionary<TurretType, TurretDynamicData> _turretDynamicData = new();
        private Dictionary<BarracksType, BarrackDynamicData> _barrackDynamicData = new();
        private WallDynamicData _wallDynamicData;

        private SceneRecourses _recourses;

        public Vector2Int GroundGridSize => _groundGridSize;
        public Vector2 StartFlagPosition => _startFlagPosition;
        public float TimeToWin => _timeToWin;
        public IReadOnlyDictionary<TurretType, TurretDynamicData> TurretDynamicData => _turretDynamicData;
        public IReadOnlyDictionary<BarracksType, BarrackDynamicData> BarrackDynamicData => _barrackDynamicData;
        public WallDynamicData WallDynamicData => _wallDynamicData; 

        [Inject]
        private void Construct(SceneRecourses recourses)
        {
            _recourses = recourses;
        }

        public void Initialize()
        {
            var turretConfigs = _recourses.Configs.TurretConfigs;
            foreach (var config in turretConfigs)
            {
                if (_turretDynamicData.ContainsKey(config.Type))
                {
                    Debug.LogError($"Duplicate configType found: {config.Type} in {config.name}");
                    continue;
                }

                var turretDynamicData = new TurretDynamicData(config);
                _turretDynamicData.Add(config.Type, turretDynamicData);
            }

            var barracksConfig = _recourses.Configs.BarracksConfigs;
            foreach (var config in barracksConfig)
            {
                if (_barrackDynamicData.ContainsKey(config.Type))
                {
                    Debug.LogError($"Duplicate configType found: {config.Type} in {config.name}");
                    continue;
                }

                var barracksDynamicData = new BarrackDynamicData(config);
                _barrackDynamicData.Add(config.Type, barracksDynamicData);
            }

            var wallConfig = _recourses.Configs.WallConfig;
            _wallDynamicData = new WallDynamicData(wallConfig);
        }
    }
}
