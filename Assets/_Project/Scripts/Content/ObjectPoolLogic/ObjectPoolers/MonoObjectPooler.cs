using Project.Content.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content
{
    public class MonoObjectPooler<T> : IFiltrablePool<T> where T : MonoBehaviour
    {
        private List<T> _objects;
        private Transform _parentTransform;
        private IPolableObjectsFactory<T> _objectsFactory;

        public MonoObjectPooler(Transform parentTransform, List<T> objects, IPolableObjectsFactory<T> objectsFactory)
        {
            _objects = objects;
            _parentTransform = parentTransform;
            _objectsFactory = objectsFactory;

            foreach (var item in objects)
            {
                item.transform.SetParent(parentTransform);
                item.gameObject.SetActive(false);
            }
        }

        public void Add(T createdObject)
        {
            _objects.Add(createdObject);
        }

        public T GetByFilter(IPoolFilterStrategy<T> filterStrategy)
        {
            var poolObject = filterStrategy.Select(_objects.ToArray());

            if (poolObject == null)
            {
                poolObject = _objectsFactory.CreateByFilter(filterStrategy);
                poolObject.transform.SetParent(_parentTransform, true);
            }

            poolObject.gameObject.SetActive(true);
            return poolObject;
        }
    }
}
