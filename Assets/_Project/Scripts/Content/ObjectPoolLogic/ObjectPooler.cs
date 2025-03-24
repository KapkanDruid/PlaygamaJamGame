using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Content
{
    public class ObjectPooler<T> where T : MonoBehaviour
    {
        private List<T> _objects;
        private GameObject _parentObject;
        private IPoolObjectsCreator<T> _poolObjectsCreator;

        public ObjectPooler(int prewarmObjects, string parentObjectName, IPoolObjectsCreator<T> poolObjectsCreator)
        {
            _parentObject = new GameObject(parentObjectName);
            _objects = new List<T>();

            _poolObjectsCreator = poolObjectsCreator;

            _objects = _poolObjectsCreator.InstantiateObjects(prewarmObjects, _parentObject);
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

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
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
