using System.Collections.Generic;
using UnityEngine;

namespace Project.Content
{
    public class GameObjectPooler
    {
        private List<GameObject> _objects;
        private GameObject _prefab;
        private Transform _parent;

        public GameObjectPooler(GameObject prefab, int prewarmCount, string parentName)
        {
            _prefab = prefab;
            _parent = new GameObject(parentName).transform;
            _objects = new List<GameObject>();

            for (int i = 0; i < prewarmCount; i++)
            {
                var obj = Create();
                obj.SetActive(false);
            }
        }

        public GameObject Get()
        {
            var obj = _objects.Find(x => !x.activeInHierarchy);
            if (obj == null)
            {
                obj = Create();
            }

            obj.SetActive(true);
            return obj;
        }

        public void Release(GameObject obj)
        {
            obj.SetActive(false);
        }

        private GameObject Create()
        {
            var obj = Object.Instantiate(_prefab, _parent);
            _objects.Add(obj);
            return obj;
        }
    }
}
