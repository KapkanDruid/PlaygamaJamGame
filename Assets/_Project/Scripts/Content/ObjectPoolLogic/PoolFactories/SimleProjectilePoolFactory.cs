using Project.Content.ProjectileSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class SimleProjectilePoolFactory : IFiltrablePoolFactory, IPolableObjectsFactory<SimpleProjectile>
    {
        private SceneData _sceneData;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<SimpleProjectile> _simpleProjectilePrefabs;

        public Type PoolType => typeof(SimpleProjectile);

        public SimleProjectilePoolFactory(SceneRecourses sceneRecourses,
                                     DiContainer container,
                                     PoolsParentContainer poolsParentContainer,
                                     SceneData sceneData)
        {
            _parentContainer = poolsParentContainer;
            _container = container;
            _sceneData = sceneData;

            _simpleProjectilePrefabs = sceneRecourses.Prefabs.SimpleProjectilePrefabs;
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
    }
}