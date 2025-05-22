using Cysharp.Threading.Tasks;
using Project.Content.ProjectileSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class SimpleProjectilePoolFactory : IFiltrablePoolFactory, IPolableObjectsFactory<SimpleProjectile>
    {
        private SceneData _sceneData;
        private SceneRecourses _sceneRecourses;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<SimpleProjectile> _simpleProjectilePrefabs = new();

        public Type PoolType => typeof(SimpleProjectile);

        public SimpleProjectilePoolFactory(SceneRecourses sceneRecourses,
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
            foreach (var prefabRef in _sceneRecourses.Prefabs.SimpleProjectilePrefabs)
            {
                var objectRef = await Addressables.LoadAssetAsync<GameObject>(prefabRef);
                var prefab = objectRef.GetComponent<SimpleProjectile>();
                if (prefab == null)
                {
                    Debug.LogError($"Prefab {objectRef.name} does not have a SimpleProjectile component!");
                    continue;
                }
                _simpleProjectilePrefabs.Add(prefab);
            }
        }

        public object Create()
        {
            List<SimpleProjectile> projectiles = new();

            var objectsCount = ObjectCountHelper.AdjustToFit(_sceneData.PoolsSize, _simpleProjectilePrefabs.Count);

            var parentTransform = _parentContainer.GetParentByType<SimpleProjectile>();

            foreach (var prefab in _simpleProjectilePrefabs)
            {
                for (int i = 0; i < objectsCount; i++)
                {
                    var createdObject = GameObject.Instantiate(prefab);
                    _container.Inject(createdObject);

                    projectiles.Add(createdObject);
                }
            }

            var pool = new MonoObjectPooler<SimpleProjectile>(parentTransform, projectiles, this);

            return pool;
        }

        public SimpleProjectile CreateByFilter(IPoolFilterStrategy<SimpleProjectile> filter)
        {
            var prefab = filter.Select(_simpleProjectilePrefabs.ToArray());

            var createdObject = GameObject.Instantiate(prefab);
            _container.Inject(createdObject);

            return createdObject;
        }

        public void Release()
        {
            foreach (var prefab in _simpleProjectilePrefabs)
            {
                if (prefab != null)
                {
                    Addressables.Release(prefab.gameObject);
                }
            }
            _simpleProjectilePrefabs.Clear();
        }
    }
}