using Cysharp.Threading.Tasks;
using Project.Content.CharacterAI.Infantryman;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Project.Content.ObjectPool
{
    public class InfantrymenPoolFactory : IFiltrablePoolFactory, IPolableObjectsFactory<InfantrymanEntity>
    {
        private SceneData _sceneData;
        private SceneRecourses _sceneRecourses;
        private DiContainer _container;
        private PoolsParentContainer _parentContainer;
        private List<InfantrymanEntity> _infantrymenPrefabs = new();

        public Type PoolType => typeof(InfantrymanEntity);

        public InfantrymenPoolFactory(SceneRecourses sceneRecourses,
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
            foreach (var prefabRef in _sceneRecourses.Prefabs.Infantrymen)
            {
                var objectRef = await Addressables.LoadAssetAsync<GameObject>(prefabRef);
                var prefab = objectRef.GetComponent<InfantrymanEntity>();
                if (prefab == null)
                {
                    Debug.LogError($"Prefab {objectRef.name} does not have a SimpleProjectile component!");
                    continue;
                }
                _infantrymenPrefabs.Add(prefab);
            }
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

        public void Release()
        {
            foreach (var prefab in _infantrymenPrefabs)
            {
                if (prefab != null)
                {
                    Addressables.Release(prefab.gameObject);
                }
            }
            _infantrymenPrefabs.Clear();
        }
    }
}