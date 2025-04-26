using Project.Content.ProjectileSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class ProjectilePoolFactory : IFiltrablePoolFactory
    {
        private SceneData _sceneData;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<SimpleProjectile> _simpleProjectilePrefabs;

        public Type PoolType => typeof(SimpleProjectile);

        public ProjectilePoolFactory(SceneRecourses sceneRecourses,
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

            var pool = new MonoObjectPooler<SimpleProjectile>(parentTransform, projectiles);

            return pool;
        }
    }
}