using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.ObjectPool
{
    public class PoolsParentContainer
    {
        private readonly Dictionary<Type, Transform> _parentObjects = new();

        private GameObject _mainParentObject;

        public void Initialize()
        {
            _mainParentObject = new GameObject("ObjectPools");
        }

        public Transform GetParentByType<T>()
        {
            var key = typeof(T);

            Transform parentObject;

            if (!_parentObjects.ContainsKey(key))
            {
                parentObject = new GameObject(key.Name + "Pool").transform;
                parentObject.SetParent(_mainParentObject.transform);

                _parentObjects.Add(key, parentObject);

                return parentObject;
            }
            else
            {
                return _parentObjects[key];
            }
        }
    }
}