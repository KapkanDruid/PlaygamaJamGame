using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Project.Content.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _prefabs;
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private int _capacityInPool;
        [SerializeField] private int _enemiesInWave;
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private float _waveInterval = 10f;

        private TSpawner<MonoBehaviour>[] _spawners;
        private CancellationToken _cancellationToken;

        private void Awake()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        private void Start()
        {
            _spawners = new TSpawner<MonoBehaviour>[_prefabs.Length];
            for (int i = 0; i < _prefabs.Length; i++)
            {
                _spawners[i] = new TSpawner<MonoBehaviour>(_prefabs[i], _capacityInPool, "Enemies");
            }

        }

        private void Update()
        {
            SpawnWaves().Forget();
        }

        private async UniTask SpawnWaves()
        {
            try
            {
                await UniTask.WaitForSeconds(_waveInterval, cancellationToken: _cancellationToken);

                for (int i = 0; i < _spawnPositions.Length; i++)
                {
                    SpawnEnemiesAtPosition(_spawnPositions[i]).Forget();
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        private async UniTask SpawnEnemiesAtPosition(Transform spawnPosition)
        {
            try
            {
                for (int i = 0; i < _spawners.Length; i++)
                {
                    for (int j = 0; j < _enemiesInWave; j++)
                    {
                        _spawners[i].Spawn(_prefabs[i].gameObject, spawnPosition);
                        await UniTask.WaitForSeconds(_spawnInterval, cancellationToken: _cancellationToken);
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
