using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class InstantiateObjectsByFactory<T> : IPoolObjectsCreator<T> where T : MonoBehaviour
    {
        private PlaceholderFactory<T> _factory;

        public InstantiateObjectsByFactory(PlaceholderFactory<T> factory)
        {
            _factory = factory;
        }

        public T Instantiate()
        {
            return _factory.Create();
        }

        public List<T> InstantiateObjects(int count, GameObject parentObject)
        {
            List<T> objects = new List<T>();

            for (int i = 0; i < count; i++)
            {
                var obj = _factory.Create();
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(parentObject.transform, true);
                objects.Add(obj);
            }

            return objects;
        }
    }
}
