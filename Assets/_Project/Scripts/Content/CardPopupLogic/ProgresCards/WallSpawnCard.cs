using Project.Content.BuildSystem;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.UI
{
    public class WallSpawnCard : CoreProgressCard, ICoreProgressStrategy
    {
        [SerializeField] private Button _button;

        private WallEntity.Factory _factory;
        private BuildingSpawner _spawner;

        private int _spawnAmount;

        public override Button Button => _button;
        public override event Action<ICoreProgressStrategy> OnCardSelected;

        [Inject]
        private void Construct(WallEntity.Factory factories, BuildingSpawner spawner, SceneRecourses recourses)
        {
            _factory = factories;
            _spawner = spawner;
            _spawnAmount = recourses.Configs.WallConfig.PlaceCardAmount;
        }

        private void Start()
        {
            _button.onClick.AddListener(() => OnCardSelected?.Invoke(this));
        }

        public void ExecuteProgress()
        {
            for (int i = 0; i < _spawnAmount; i++)
            {
                _spawner.Spawn(_factory);
            }
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
