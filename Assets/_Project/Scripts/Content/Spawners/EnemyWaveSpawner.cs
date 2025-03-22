using Cysharp.Threading.Tasks;
using Project.Content.CharacterAI;
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
        private int _currentSpawnerIndex = 0;
        private List<IEnemySpawner> _enemySpawners = new();
        private DestroyerSpawner.Factory _destroyerSpawnerFactory;
        private MainTargetAttackerSpawner.Factory _mainTargetAttackerSpawnerFactory;
        private Wave _currentWave;
        private IEnemySpawner _spawner;

        [Inject]
        private void Construct(DestroyerSpawner.Factory destroyerSpawner, MainTargetAttackerSpawner.Factory mainTargetAttackerSpawner)
        {
            _destroyerSpawnerFactory = destroyerSpawner;
            _mainTargetAttackerSpawnerFactory = mainTargetAttackerSpawner;


        }

        private void Initialize()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();


            _enemySpawners.Add(_destroyerSpawnerFactory.Create(DestroyerType.SimpleParanoid));
            _enemySpawners.Add(_destroyerSpawnerFactory.Create(DestroyerType.AdvencedParanoid));
            _enemySpawners.Add(_destroyerSpawnerFactory.Create(DestroyerType.FlatEarther));
            _enemySpawners.Add(_destroyerSpawnerFactory.Create(DestroyerType.Aliens));

            _enemySpawners.Add(_mainTargetAttackerSpawnerFactory.Create(MainTargetAttackerType.Bigfoot));
            _enemySpawners.Add(_mainTargetAttackerSpawnerFactory.Create(MainTargetAttackerType.HumanMoth));

            foreach (var spawner in _enemySpawners)
            {
                spawner.Initialize(_waves[0].EnemyGroups.Count);
            }

            foreach (var wave in _waves)
            {
                foreach (var spawnPosition in wave.SpawnPositions)
                {
                    spawnPosition.Initialize();
                }
            }

            _spawner = _enemySpawners[_currentSpawnerIndex];
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
                        if (group.Prefab.GetType() == _spawner.GetTypeObject())
                        {
                            await UniTask.WaitForSeconds(_spawnInterval, cancellationToken: _cancellationToken);

                            _spawner.Spawn(_currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].Points[_currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].CurrentSpawnPointIndex].position);
                            NextSpawnPoint();
                        }
                        else
                        {
                            NextSpawner();

                            if (j > 0)
                                j--;
                        }
                    }
                    NextSpawner();
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        private void NextSpawner()
        {
            if (_enemySpawners.Count <= 1)
                return;

            _currentSpawnerIndex++;

            if (_currentSpawnerIndex >= _enemySpawners.Count)
            {
                _currentSpawnerIndex = 0;
            }

            _spawner = _enemySpawners[_currentSpawnerIndex];
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
