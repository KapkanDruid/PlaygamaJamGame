using Project.Content.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Content
{
    public class MonoObjectPooler<T> : IFiltrablePool<T> where T : MonoBehaviour
    {
        private List<T> _objects;
        private GameObject _parentObject;
        private Transform _parentTransform;
        private IPoolObjectsCreator<T> _poolObjectsCreator;
        private IPolableObjectsFactory<T> _objectsFactory;

        public MonoObjectPooler(int prewarmObjects, string parentObjectName, IPoolObjectsCreator<T> poolObjectsCreator)
        {
            _parentObject = new GameObject(parentObjectName);
            _objects = new List<T>();

            _poolObjectsCreator = poolObjectsCreator;

            _objects = _poolObjectsCreator.InstantiateObjects(prewarmObjects, _parentObject);
        }

        public MonoObjectPooler(int prewarmObjects, GameObject parentObject, IPoolObjectsCreator<T> poolObjectsCreator)
        {
            _parentObject = parentObject;
            _objects = new List<T>();

            _poolObjectsCreator = poolObjectsCreator;

            _objects = _poolObjectsCreator.InstantiateObjects(prewarmObjects, _parentObject);
        }

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

        public T Get()
        {
            var obj = _objects.FirstOrDefault(x => !x.isActiveAndEnabled);

            if (obj == null)
            {
                obj = Create();
            }

            obj.gameObject.SetActive(true);
            return obj;
        }

        private T Create()
        {
            var obj = _poolObjectsCreator.Instantiate();
            _objects.Add(obj);

            if (_parentObject != null)
            {
                obj.transform.SetParent(_parentObject.transform, true);
            }
            return obj;
        }
    }
}
