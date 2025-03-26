using Project.Architecture;
using Project.Content.BuildSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.UI
{
    public class BarrackSpawnCard : CoreProgressCard, ICoreProgressStrategy
    {
        [SerializeField] private BarracksType _type;
        [SerializeField] private Button _button;

        [Header("Tex Fields Original"), Space(3)]
        [SerializeField] private TextMeshProUGUI _capacityOriginal;
        [SerializeField] private TextMeshProUGUI _spawnCooldownOriginal;
        [SerializeField] private TextMeshProUGUI _healthOriginal;

        [Header("Tex Fields Modified"), Space(3)]
        [SerializeField] private TextMeshProUGUI _healthModified;

        [Header("Tex Fields Summarized"), Space(3)]
        [SerializeField] private TextMeshProUGUI _healthSummarized;

        private List<BarracksEntity.Factory> _factories;
        private BarracksEntity.Factory _rightFactory;

        private BuildingSpawner _spawner;
        private BarrackDynamicData _dynamicData;
        private BarrackConfig _config;
        private SceneData _sceneData;

        public override Button Button => _button;
        public override event Action<ICoreProgressStrategy> OnCardSelected;

        [Inject]
        private void Construct(List<BarracksEntity.Factory> factories, BuildingSpawner spawner, SceneData sceneData)
        {
            _factories = factories;
            _spawner = spawner;
            _sceneData = sceneData;

            MainSceneBootstrap.OnServicesInitialized += Initialize;
        }

        private void Initialize()
        {
            _button.onClick.AddListener(() => OnCardSelected?.Invoke(this));

            if (_healthOriginal == null)
                return;

            _dynamicData = _sceneData.BarrackDynamicData[_type];
            _config = _dynamicData.Config;

            _capacityOriginal.text = _config.Capacity.ToString();
            _spawnCooldownOriginal.text = _config.SpawnCooldown.ToString();
            _healthOriginal.text = _config.BuildingMaxHealth.ToString();

            _healthModified.text = (_dynamicData.BuildingMaxHealth.Value - _config.BuildingMaxHealth).ToString();

            _healthSummarized.text = _dynamicData.BuildingMaxHealth.Value.ToString();
        }

        public void ExecuteProgress()
        {
            foreach (var factory in _factories)
            {
                if (factory.Type == _type)
                {
                    _rightFactory = factory;
                    break;
                }
            }

            if (_rightFactory == null)
            {
                Debug.LogError("[BarrackSpawnCard] did not get factory with type " + _type);
                return;
            }

            _spawner.Spawn(_rightFactory);
        }

        private void OnEnable()
        {
            if (_healthOriginal == null)
                return;

            _healthModified.text = (_dynamicData.BuildingMaxHealth.Value - _config.BuildingMaxHealth).ToString();

            _healthSummarized.text = _dynamicData.BuildingMaxHealth.Value.ToString();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            MainSceneBootstrap.OnServicesInitialized += Initialize;
        }
    }
}
