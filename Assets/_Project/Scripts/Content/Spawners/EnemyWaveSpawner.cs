using Cysharp.Threading.Tasks;
using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Destroyer;
using Project.Content.CharacterAI.MainTargetAttacker;
using Project.Content.ObjectPool;
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
        private PauseHandler _pauseHandler;
        private Wave _currentWave;
        private FiltrablePoolsHandler _poolsHandler;

        private DestroyerType _simpleParanoidData = DestroyerType.SimpleParanoid;
        private DestroyerType _advencedParanoidData = DestroyerType.AdvencedParanoid;
        private DestroyerType _flatEartherData = DestroyerType.FlatEarther;
        private DestroyerType _aliensData = DestroyerType.Aliens;

        private MainTargetAttackerType _bigfootData = MainTargetAttackerType.Bigfoot;
        private MainTargetAttackerType _humanMothData = MainTargetAttackerType.HumanMoth;

        public event Action<Transform> OnSpawnPointSelected;

        private Predicate<DestroyerEntity> SimpleParanoidPredicate
            => item => item.Type == _simpleParanoidData && item.gameObject.activeInHierarchy == false;
        private Predicate<DestroyerEntity> AdvencedParanoidPredicate
            => item => item.Type == _advencedParanoidData && item.gameObject.activeInHierarchy == false;
        private Predicate<DestroyerEntity> FlatEatherPredicate
            => item => item.Type == _flatEartherData && item.gameObject.activeInHierarchy == false;
        private Predicate<DestroyerEntity> AliensPredicate
            => item => item.Type == _aliensData && item.gameObject.activeInHierarchy == false;

        private Predicate<MainTargetAttackerEntity> BigfootPredicate
            => item => item.Type == _bigfootData && item.gameObject.activeInHierarchy == false;
        private Predicate<MainTargetAttackerEntity> HumanMothPredicate
            => item => item.Type == _humanMothData && item.gameObject.activeInHierarchy == false;

        private Dictionary<Type, Func<MonoBehaviour, (Type, Delegate)>> _enemyTypeResolvers;

        private void InitializeEnemyTypeResolvers()
        {
            _enemyTypeResolvers = new()
            {
                {
                    typeof(DestroyerEntity), prefab =>
                    {
                        var destroyer = prefab as DestroyerEntity;
                        return destroyer.Type switch
                        {
                            DestroyerType.SimpleParanoid => (typeof(DestroyerEntity), SimpleParanoidPredicate),
                            DestroyerType.AdvencedParanoid => (typeof(DestroyerEntity), AdvencedParanoidPredicate),
                            DestroyerType.FlatEarther => (typeof(DestroyerEntity), FlatEatherPredicate),
                            DestroyerType.Aliens => (typeof(DestroyerEntity), AliensPredicate),
                            _ => throw new Exception("Unknown DestroyerType")
                        };
                    }
                },
                {
                    typeof(MainTargetAttackerEntity), prefab =>
                    {
                        var attacker = prefab as MainTargetAttackerEntity;
                        return attacker.Type switch
                        {
                            MainTargetAttackerType.Bigfoot => (typeof(MainTargetAttackerEntity), BigfootPredicate),
                            MainTargetAttackerType.HumanMoth => (typeof(MainTargetAttackerEntity), HumanMothPredicate),
                            _ => throw new Exception("Unknown MainTargetAttackerType")
                        };
                    }
                }
            };
        }

        [Inject]
        private void Construct(PauseHandler pauseHandler,
                           FiltrablePoolsHandler poolsHandler = null)
        {
            _pauseHandler = pauseHandler;
            _poolsHandler = poolsHandler;

        }

        private void Initialize()
        {
            InitializeEnemyTypeResolvers();

            _cancellationToken = this.GetCancellationTokenOnDestroy();

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

                if (_pauseHandler.IsPaused)
                {
                    await UniTask.WaitUntil(() => !_pauseHandler.IsPaused, cancellationToken: _cancellationToken);
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
                    for (int j = 0; j < group.Count;)
                    {
                        if (_pauseHandler.IsPaused)
                        {
                            await UniTask.WaitUntil(() => !_pauseHandler.IsPaused, cancellationToken: _cancellationToken);
                        }

                        await UniTask.WaitForSeconds(_spawnInterval, cancellationToken: _cancellationToken);

                        var spawnPoint = _currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].Points[_currentWave.SpawnPositions[_currentWave.CurrentSpawnPositionIndex].CurrentSpawnPointIndex];
                        OnSpawnPointSelected?.Invoke(spawnPoint);

                        NextSpawnPoint();

                        var enemy = GetEnemyFromPool(group.Prefab);

                        enemy.transform.position = spawnPoint.position;
                        
                        j++;

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

        private CharacterHandler GetEnemyFromPool(MonoBehaviour prefab)
        {
            foreach (var enemyTypeByPredicate in _enemyTypeResolvers)
            {
                if (enemyTypeByPredicate.Key.IsInstanceOfType(prefab))
                {
                    var (type, predicate) = enemyTypeByPredicate.Value(prefab);

                    if (type == typeof(DestroyerEntity))
                        return _poolsHandler.GetByPredicate((Predicate<DestroyerEntity>)predicate);
                    if (type == typeof(MainTargetAttackerEntity))
                        return _poolsHandler.GetByPredicate((Predicate<MainTargetAttackerEntity>)predicate);
                }
            }
            Debug.LogError($"No resolver found for prefab {prefab}");
            return null;
        }
    }
}
