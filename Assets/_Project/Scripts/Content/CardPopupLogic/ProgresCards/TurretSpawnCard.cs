using Project.Content.BuildSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.UI
{
    public class TurretSpawnCard : CoreProgressCard, ICoreProgressStrategy
    {
        [SerializeField] private TurretType _turretType;
        [SerializeField] private Button _button;

        [Header("Tex Fields Original"), Space(3)]
        [SerializeField] private TextMeshProUGUI _damageOriginal;
        [SerializeField] private TextMeshProUGUI _cooldownOriginal;
        [SerializeField] private TextMeshProUGUI _healthOriginal;
        [SerializeField] private TextMeshProUGUI _attackAreaOriginal;

        [Header("Tex Fields Modified"), Space(3)]
        [SerializeField] private TextMeshProUGUI _healthModified;
        [SerializeField] private TextMeshProUGUI _attackAreaModified;

        [Header("Tex Fields Summarized"), Space(3)]
        [SerializeField] private TextMeshProUGUI _healthSummarized;
        [SerializeField] private TextMeshProUGUI _attackAreaSummarized;

        private List<TurretEntity.Factory> _factories;
        private TurretEntity.Factory _rightFactory;

        private BuildingSpawner _spawner;
        private TurretDynamicData _dynamicData;
        private TurretConfig _config;
        private SceneData _sceneData;

        public override Button Button => _button;
        public override event Action<ICoreProgressStrategy> OnCardSelected;

        [Inject]
        private void Construct(List<TurretEntity.Factory> factories, BuildingSpawner spawner, SceneData sceneData)
        {
            _factories = factories;
            _spawner = spawner;
            _sceneData = sceneData;
        }

        private void Start()
        {
            _button.onClick.AddListener(() => OnCardSelected?.Invoke(this));

            if (_healthOriginal == null)
                return;

            _dynamicData = _sceneData.TurretDynamicData[_turretType];
            _config = _dynamicData.Config;

            _damageOriginal.text = _config.ProjectileDamage.ToString();
            _cooldownOriginal.text = _config.FireRate.ToString();
            _healthOriginal.text = _config.MaxHealth.ToString();
            _attackAreaOriginal.text = _config.SensorRadius.ToString();

            _healthModified.text = (_dynamicData.MaxHealth.Value - _config.MaxHealth).ToString();
            _attackAreaModified.text = (_dynamicData.SensorRadius.Value - _config.SensorRadius).ToString();
        }

        public void ExecuteProgress()
        {
            foreach (var factory in _factories)
            {
                if (factory.Type == _turretType)
                {
                    _rightFactory = factory;
                    break;
                }
            }

            if (_rightFactory == null)
            {
                Debug.LogError("TurretSpawnCard did not get factory with type " + _turretType);
                return;
            }

            _spawner.Spawn(_rightFactory);
        }

        private void OnEnable()
        {
            if (_healthOriginal == null)
                return;

            _healthModified.text = (_dynamicData.MaxHealth.Value - _config.MaxHealth).ToString();
            _attackAreaModified.text = (_dynamicData.SensorRadius.Value - _config.SensorRadius).ToString();

            _healthSummarized.text = _dynamicData.MaxHealth.Value.ToString();
            _attackAreaSummarized.text = _dynamicData.SensorRadius.Value.ToString();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
