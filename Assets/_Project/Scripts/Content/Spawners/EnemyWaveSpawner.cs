using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Content.Spawners
{
    public class EnemyWaveSpawner : MonoBehaviour
    {
        [Serializable]
        public class Wave
        {
            public List<EnemyGroup> EnemyGroups;
            public List<SpawnPoint> SpawnPositions;
            public float WaveInterval = 10f;
            [HideInInspector]
            public int CurrentSpawnPositionIndex = 0;
        }

        [Serializable]
        public class EnemyGroup
        {
            public MonoBehaviour Prefab;
            public int Count;
        }

        [Serializable]
        public class SpawnPoint
        {
            public List<Transform> Points;
            [HideInInspector]
            public int CurrentSpawnPointIndex = 0;

            public void Initialize()
            {
                List<Transform> points = new();

                foreach (var point in Points)
                {
                    var chieldPoints = point.GetComponentsInChildren<Transform>();

                    var pointsToAdd = chieldPoints.Where(item => !Points.Contains(item));
                    points.AddRange(pointsToAdd);
                }

                Points = points;

            }
        }

        [SerializeField] private List<Wave> _waves;
        [SerializeField] private float _spawnInterval = 2f;

        private CancellationToken _cancellationToken;
        private int _currentWaveIndex = 0;
        private List<IEnemySpawner> _enemySpawners = new();
        private DestroyerSpawner _destroyerSpawner;
        private MainTargetAttackerSpawner _mainTargetAttackerSpawner;
        private Wave _currentWave;

        [Inject]
        private void Construct(DestroyerSpawner destroyerSpawner, MainTargetAttackerSpawner mainTargetAttackerSpawner)
        {
            _destroyerSpawner = destroyerSpawner;
            _mainTargetAttackerSpawner = mainTargetAttackerSpawner;

            _enemySpawners.Add(_destroyerSpawner);
            _enemySpawners.Add(_mainTargetAttackerSpawner);

        }

        private void Initialize()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();

            foreach (var spawner in _enemySpawners)
            {
                spawner.Initialize();
            }

            foreach (var wave in _waves)
            {
                foreach (var spawnPosition in wave.SpawnPositions)
                {
                    spawnPosition.Initialize();
                }
            }

        }

        private void Start()
        {
            Initialize();

            SpawnWaves().Forget();
        }


        private async UniTask SpawnWaves()
        {
            try
            {
                if (_currentWaveIndex >= _waves.Count)
                {
                    Debug.Log("Волны закончились!");
                    return;
                }

                await UniTask.WaitForSeconds(_waves[_currentWaveIndex].WaveInterval, cancellationToken: _cancellationToken);

                if (_currentWaveIndex < _waves.Count)
                {
                    _currentWave = _waves[_currentWaveIndex];
                    await SpawnEnemiesAtPosition(_currentWave);
                    _currentWaveIndex++;
                    SpawnWaves().Forget();
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
                for (int i = 0; i < wave.EnemyGroups.Count; i++)
                {
                    EnemyGroup group = wave.EnemyGroups[i];
                    for (int j = 0; j < group.Count; j++)
                    {
                        for (int k = 0; k < _enemySpawners.Count; k++)
                        {
                            IEnemySpawner spawner = _enemySpawners[k];

                            if (group.Prefab.GetType() == spawner.GetTypeObject())
                            {
                                await UniTask.WaitForSeconds(_spawnInterval, cancellationToken: _cancellationToken);

                                spawner.Spawn(_currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].Points[_currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].CurrentSpawnPointIndex].position);
                                NextSpawnPoint();
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        private void NextSpawnPoint()
        {
            if (_currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].Points.Count <= 1)
                return;

            _currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].CurrentSpawnPointIndex++;
            if (_currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].CurrentSpawnPointIndex >= _currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].Points.Count)
            {
                _currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].CurrentSpawnPointIndex = 0;
                NextSpawnPoisition();
            }
        }

        private void NextSpawnPoisition()
        {
            if (_currentWave.SpawnPositions.Count <= 1)
                return;

            _currentWave.CurrentSpawnPositionIndex++;
            if (_currentWave.CurrentSpawnPositionIndex >= _currentWave.SpawnPositions.Count)
            {
                _currentWave.CurrentSpawnPositionIndex = 0;
            }
        }
    }
}
