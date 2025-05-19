using Project.Content.CharacterAI.Infantryman;
using System;
using System.Collections.Generic;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class InfantrymenPoolFactory : IFiltrablePoolFactory, IPolableObjectsFactory<InfantrymanEntity>
    {
        private SceneData _sceneData;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<InfantrymanEntity> _infantrymenPrefabs;

        public Type PoolType => typeof(InfantrymanEntity);

        public InfantrymenPoolFactory(SceneRecourses sceneRecourses,
                                     DiContainer container,
                                     PoolsParentContainer poolsParentContainer,
                                     SceneData sceneData)
        {
            _parentContainer = poolsParentContainer;
            _container = container;
            _sceneData = sceneData;

            _infantrymenPrefabs = sceneRecourses.Prefabs.Infantrymen;
        }

        public object Create()
        {
            List<InfantrymanEntity> projectiles = new();

            var objectsCount = ObjectCountHelper.AdjustToFit(_sceneData.PoolsSize, _infantrymenPrefabs.Count);

            var parentTransform = _parentContainer.GetParentByType<InfantrymanEntity>();

            foreach (var prefab in _infantrymenPrefabs)
            {
                for (int i = 0; i < objectsCount; i++)
                {
                    var createdObject = _container.InstantiatePrefabForComponent<InfantrymanEntity>(prefab, parentTransform);
                    projectiles.Add(createdObject);
                }
            }

            var pool = new MonoObjectPooler<InfantrymanEntity>(parentTransform, projectiles, this);

            return pool;
        }

        public InfantrymanEntity CreateByFilter(IPoolFilterStrategy<InfantrymanEntity> filter)
        {
            var prefab = filter.Select(_infantrymenPrefabs.ToArray());

            var parentTransform = _parentContainer.GetParentByType<InfantrymanEntity>();

            var createdObject = _container.InstantiatePrefabForComponent<InfantrymanEntity>(prefab, parentTransform);

            return createdObject;
        }
    }
}