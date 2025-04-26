using System.Collections.Generic;
using UnityEngine;

namespace Project.Content
{
    public class InstantiateObjectsSimple<T> : IPoolObjectsCreator<T> where T : MonoBehaviour
    {
        private T _prefab;

        public InstantiateObjectsSimple(T prefab)
        {
            _prefab = prefab;
        }

        public T Instantiate()
        {
            return GameObject.Instantiate(_prefab);
        }

        public List<T> InstantiateObjects(int count, GameObject parentObject)
        {
            List<T> objects = new List<T>();

            for (int i = 0; i < count; i++)
            {
                var obj = GameObject.Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(parentObject.transform, true);
                objects.Add(obj);
            }

            return objects;
        }
    }

}
