using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Content
{
    public class ObjectPooler<T> where T : MonoBehaviour
    {
        private T _prefab;
        private List<T> _objects;
        private GameObject _parentObject;

        public ObjectPooler(T prefab, int prewarmObjects, string parentObjectName)
        {
            _parentObject = new GameObject(parentObjectName);
            _prefab = prefab;
            _objects = new List<T>();

            for (int i = 0; i < prewarmObjects; i++)
            {
                var obj = GameObject.Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(_parentObject.transform, true);
                _objects.Add(obj);
            }

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
            var obj = GameObject.Instantiate(_prefab);
            _objects.Add(obj);

            if (_parentObject != null)
            {
                obj.transform.SetParent(_parentObject.transform, true);
            }
            return obj;
        }
    }
}
