using Project.Content.BuildSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.UI
{
    public class BarrackSpawnCard : CoreProgressCard, ICoreProgressStrategy
    {
        [SerializeField] private BarracksType _turretType;
        [SerializeField] private Button _button;

        private List<BarracksEntity.Factory> _factories;
        private BarracksEntity.Factory _rightFactory;

        private BuildingSpawner _spawner;

        public override Button Button => _button;
        public override event Action<ICoreProgressStrategy> OnCardSelected;

        [Inject]
        private void Construct(List<BarracksEntity.Factory> factories, BuildingSpawner spawner)
        {
            _factories = factories;
            _spawner = spawner;
        }

        private void Start()
        {
            _button.onClick.AddListener(() => OnCardSelected?.Invoke(this));
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
                Debug.LogError("[BarrackSpawnCard] did not get factory with type " + _turretType);
                return;
            }

            _spawner.Spawn(_rightFactory);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
