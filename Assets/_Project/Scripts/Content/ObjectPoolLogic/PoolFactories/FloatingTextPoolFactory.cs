using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.ObjectPool
{
    public class FloatingTextPoolFactory : IPolableObjectsFactory<FloatingText>
    {
        private PoolsParentContainer _parentContainer;
        private SceneData _sceneData;

        private FloatingText[] _prefabs;

        public FloatingTextPoolFactory(SceneRecourses sceneRecourses,
                                     PoolsParentContainer poolsParentContainer,
                                     SceneData sceneData)
        {
            _parentContainer = poolsParentContainer;
            _sceneData = sceneData;

            _prefabs = sceneRecourses.Prefabs.FloatingTextPrefabs;

            var config = _prefabs[0].Config;

            for (int i = 1; i < _prefabs.Length; i++)
            {
                if (config == _prefabs[i].Config)
                    Debug.LogError($"[FloatingTextPoolFactory] Filed to create {config.name} object! Found multiple configs with same type.");
            }
        }

        public MonoObjectPooler<FloatingText> Create()
        {
            List<FloatingText> objects = new();

            var objectsCount = ObjectCountHelper.AdjustToFit(_sceneData.PoolsSize, _prefabs.Length);

            var parentTransform = _parentContainer.GetParentByType<FloatingText>();

            foreach (var prefab in _prefabs)
            {
                for (int i = 0; i < objectsCount; i++)
                {
                    var createdObject = GameObject.Instantiate(prefab);

                    objects.Add(createdObject);
                }
            }

            var pool = new MonoObjectPooler<FloatingText>(parentTransform, objects, this);

            return pool;
        }

        public FloatingText CreateByFilter(IPoolFilterStrategy<FloatingText> filter)
        {
            var prefab = filter.Select(_prefabs);
            var createdObject = GameObject.Instantiate(prefab);
            return createdObject;
        }
    }
}