using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Project.Content.Spawners
{
    public class EnemyFactory : MonoBehaviour
    {
        [Serializable]
        public class Wave
        {
            public List<EnemyGroup> EnemyGroups;
        }

        [Serializable]
        public class EnemyGroup
        {
            public MonoBehaviour Prefab;
            public int Count;
        }

        [SerializeField] private List<Wave> _waves;
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private int _capacityInPool;
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private float _waveInterval = 10f;

        private Dictionary<MonoBehaviour, Spawner<MonoBehaviour>> _spawners;
        private CancellationToken _cancellationToken;
        private int _currentWaveIndex = 0;

        private void Awake()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            InitializeSpawners();
            SpawnWaves().Forget();
        }

        private void InitializeSpawners()
        {
            _spawners = new Dictionary<MonoBehaviour, Spawner<MonoBehaviour>>();
            foreach (var wave in _waves)
            {
                foreach (var group in wave.EnemyGroups)
                {
                    if (!_spawners.ContainsKey(group.Prefab))
                    {
                        _spawners[group.Prefab] = new Spawner<MonoBehaviour>(group.Prefab, _capacityInPool, "Enemies");
                    }
                }
            }

        }

        private async UniTask SpawnWaves()
        {
            try
            {
                await UniTask.WaitForSeconds(_waveInterval, cancellationToken: _cancellationToken);

                if (_currentWaveIndex < _waves.Count)
                {
                    var currentWave = _waves[_currentWaveIndex];
                    await SpawnEnemiesAtPosition(currentWave);
                    _currentWaveIndex++;
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        private async UniTask SpawnEnemiesAtPosition(Wave wave)
        {
            try
            {
                foreach (var group in wave.EnemyGroups)
                {
                    for (int i = 0; i < group.Count; i++)
                    {
                        var spawner = _spawners[group.Prefab];
                        await UniTask.WaitForSeconds(_spawnInterval, cancellationToken: _cancellationToken);
                        spawner.Spawn(_spawnPositions[UnityEngine.Random.Range(0, _spawnPositions.Length)]);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}
