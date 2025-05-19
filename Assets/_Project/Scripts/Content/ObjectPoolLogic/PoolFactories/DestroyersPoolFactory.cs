using Project.Content.CharacterAI.Destroyer;
using System;
using System.Collections.Generic;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class DestroyersPoolFactory : IFiltrablePoolFactory, IPolableObjectsFactory<DestroyerEntity>
    {
        private SceneData _sceneData;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<DestroyerEntity> _destroyersPrefabs;

        public Type PoolType => typeof(DestroyerEntity);

        public DestroyersPoolFactory(SceneRecourses sceneRecourses,
                                     DiContainer container,
                                     PoolsParentContainer poolsParentContainer,
                                     SceneData sceneData)
        {
            _parentContainer = poolsParentContainer;
            _container = container;
            _sceneData = sceneData;

            _destroyersPrefabs = sceneRecourses.Prefabs.DestroyersPrefabs;
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
    }
}