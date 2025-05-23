using Cysharp.Threading.Tasks;
using Project.Content.CharacterAI.Destroyer;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class DestroyersPoolFactory : IFiltrablePoolFactory, IPolableObjectsFactory<DestroyerEntity>
    {
        private SceneData _sceneData;
        private SceneRecourses _sceneRecourses;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<DestroyerEntity> _destroyersPrefabs = new();

        public Type PoolType => typeof(DestroyerEntity);

        public DestroyersPoolFactory(SceneRecourses sceneRecourses,
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
            foreach (var prefabRef in _sceneRecourses.Prefabs.DestroyersPrefabs)
            {
                var objectRef = await Addressables.LoadAssetAsync<GameObject>(prefabRef);
                var prefab = objectRef.GetComponent<DestroyerEntity>();
                if (prefab == null)
                {
                    Debug.LogError($"Prefab {objectRef.name} does not have a SimpleProjectile component!");
                    continue;
                }
                _destroyersPrefabs.Add(prefab);
            }
        }

        public object Create()
        {
            List<DestroyerEntity> projectiles = new();

            var objectsCount = ObjectCountHelper.AdjustToFit(_sceneData.PoolsSize, _destroyersPrefabs.Count);

            var parentTransform = _parentContainer.GetParentByType<DestroyerEntity>();

            foreach (var prefab in _destroyersPrefabs)
            {
                for (int i = 0; i < objectsCount; i++)
                {
                    var createdObject = _container.InstantiatePrefabForComponent<DestroyerEntity>(prefab, parentTransform);
                    projectiles.Add(createdObject);
                }
            }

            var pool = new MonoObjectPooler<DestroyerEntity>(parentTransform, projectiles, this);

            return pool;
        }

        public DestroyerEntity CreateByFilter(IPoolFilterStrategy<DestroyerEntity> filter)
        {
            var prefab = filter.Select(_destroyersPrefabs.ToArray());

            var parentTransform = _parentContainer.GetParentByType<DestroyerEntity>();

            var createdObject = _container.InstantiatePrefabForComponent<DestroyerEntity>(prefab, parentTransform);

            return createdObject;
        }

        public void Release()
        {
            foreach (var prefab in _destroyersPrefabs)
            {
                if (prefab != null)
                {
                    Addressables.Release(prefab.gameObject);
                }
            }
            _destroyersPrefabs.Clear();
        }
    }
}