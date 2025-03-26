using Project.Content.BuildSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.UI
{
    public class WallSpawnCard : CoreProgressCard, ICoreProgressStrategy
    {
        [SerializeField] private Button _button;

        [Header("Tex Fields Original"), Space(3)]
        [SerializeField] private TextMeshProUGUI _healthOriginal;

        [Header("Tex Fields Modified"), Space(3)]
        [SerializeField] private TextMeshProUGUI _healthModified;

        [Header("Tex Fields Summarized"), Space(3)]
        [SerializeField] private TextMeshProUGUI _healthSummarized;

        private WallEntity.Factory _factory;
        private BuildingSpawner _spawner;
        private WallDynamicData _dynamicData;
        private WallConfig _config;
        private SceneData _sceneData;

        private int _spawnAmount;

        public override Button Button => _button;
        public override event Action<ICoreProgressStrategy> OnCardSelected;

        [Inject]
        private void Construct(WallEntity.Factory factories, BuildingSpawner spawner, SceneData sceneData)
        {
            _factory = factories;
            _spawner = spawner;
            _sceneData = sceneData;
        }

        private void Start()
        {
            _button.onClick.AddListener(() => OnCardSelected?.Invoke(this));

            if (_healthOriginal == null)
                return;

            _dynamicData = _sceneData.WallDynamicData;
            _config = _dynamicData.Config;

            _healthOriginal.text = _config.MaxHealth.ToString();
            _healthModified.text = (_dynamicData.BuildingMaxHealth.Value - _config.MaxHealth).ToString();
            _healthSummarized.text = _config.MaxHealth.ToString();
        }

        public void ExecuteProgress()
        {
            for (int i = 0; i < _spawnAmount; i++)
            {
                _spawner.Spawn(_factory);
            }
        }

        private void OnEnable()
        {
            if (_healthOriginal == null)
                return;

            _healthModified.text = (_dynamicData.BuildingMaxHealth.Value - _config.MaxHealth).ToString();
            _healthSummarized.text = _config.MaxHealth.ToString();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
