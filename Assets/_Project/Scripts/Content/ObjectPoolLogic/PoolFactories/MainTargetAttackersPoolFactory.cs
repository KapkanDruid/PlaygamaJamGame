using Project.Content.CharacterAI.MainTargetAttacker;
using System;
using System.Collections.Generic;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class MainTargetAttackersPoolFactory : IFiltrablePoolFactory, IPolableObjectsFactory<MainTargetAttackerEntity>
    {
        private SceneData _sceneData;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<MainTargetAttackerEntity> _mainTargetAttackersPrefabs;

        public Type PoolType => typeof(MainTargetAttackerEntity);

        public MainTargetAttackersPoolFactory(SceneRecourses sceneRecourses,
                                     DiContainer container,
                                     PoolsParentContainer poolsParentContainer,
                                     SceneData sceneData)
        {
            _parentContainer = poolsParentContainer;
            _container = container;
            _sceneData = sceneData;

            _mainTargetAttackersPrefabs = sceneRecourses.Prefabs.MainTargetAttackersPrefabs;
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
    }
}