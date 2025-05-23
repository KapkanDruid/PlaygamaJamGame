using Cysharp.Threading.Tasks;
using Project.Content.CharacterAI.MainTargetAttacker;
using Project.Content.ProjectileSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class MainTargetAttackersPoolFactory : IFiltrablePoolFactory, IPolableObjectsFactory<MainTargetAttackerEntity>
    {
        private SceneData _sceneData;
        private SceneRecourses _sceneRecourses;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<MainTargetAttackerEntity> _mainTargetAttackersPrefabs = new();

        public Type PoolType => typeof(MainTargetAttackerEntity);

        public MainTargetAttackersPoolFactory(SceneRecourses sceneRecourses,
                                     DiContainer container,
                                     PoolsParentContainer poolsParentContainer,
                                     SceneData sceneData)
        {
            _parentContainer = poolsParentContainer;
            _container = container;
            _sceneData = sceneData;
            _sceneRecourses = sceneRecourses;
        }

        public async UniTask PreloadAsync()
        {
            foreach (var prefabRef in _sceneRecourses.Prefabs.MainTargetAttackersPrefabs)
            {
                var objectRef = await Addressables.LoadAssetAsync<GameObject>(prefabRef);
                var prefab = objectRef.GetComponent<MainTargetAttackerEntity>();
                if (prefab == null)
                {
                    Debug.LogError($"Prefab {objectRef.name} does not have a SimpleProjectile component!");
                    continue;
                }
                _mainTargetAttackersPrefabs.Add(prefab);
            }
        }

        public object Create()
        {
            List<MainTargetAttackerEntity> projectiles = new();

            var objectsCount = ObjectCountHelper.AdjustToFit(_sceneData.PoolsSize, _mainTargetAttackersPrefabs.Count);

            var parentTransform = _parentContainer.GetParentByType<MainTargetAttackerEntity>();

            foreach (var prefab in _mainTargetAttackersPrefabs)
            {
                for (int i = 0; i < objectsCount; i++)
                {
                    var createdObject = _container.InstantiatePrefabForComponent<MainTargetAttackerEntity>(prefab, parentTransform);

                    projectiles.Add(createdObject);
                }
            }

            var pool = new MonoObjectPooler<MainTargetAttackerEntity>(parentTransform, projectiles, this);

            return pool;
        }

        public MainTargetAttackerEntity CreateByFilter(IPoolFilterStrategy<MainTargetAttackerEntity> filter)
        {
            var prefab = filter.Select(_mainTargetAttackersPrefabs.ToArray());
            var parentTransform = _parentContainer.GetParentByType<MainTargetAttackerEntity>();

            var createdObject = _container.InstantiatePrefabForComponent<MainTargetAttackerEntity>(prefab, parentTransform);

            return createdObject;
        }

        public void Release()
        {
            foreach (var prefab in _mainTargetAttackersPrefabs)
            {
                if (prefab != null)
                {
                    Addressables.Release(prefab.gameObject);
                }
            }
            _mainTargetAttackersPrefabs.Clear();
        }
    }
}