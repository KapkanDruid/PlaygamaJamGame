using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class InstantiateObjectContainer<T> : IPoolObjectsCreator<T> where T : MonoBehaviour
    {
        private T _prefab;
        private DiContainer _diContainer;

        public InstantiateObjectContainer(T prefab, DiContainer diContainer)
        {
            _prefab = prefab;
            _diContainer = diContainer;
        }

        public T Instantiate()
        {
            var obj = GameObject.Instantiate(_prefab);
            _diContainer.Inject(obj);

            return obj;
        }

        public List<T> InstantiateObjects(int count, GameObject parentObject)
        {
            List<T> objects = new List<T>();

            for (int i = 0; i < count; i++)
            {
                var obj = GameObject.Instantiate(_prefab);
                _diContainer.Inject(obj);
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(parentObject.transform, true);
                objects.Add(obj);
            }

            return objects;
        }
    } 

}
