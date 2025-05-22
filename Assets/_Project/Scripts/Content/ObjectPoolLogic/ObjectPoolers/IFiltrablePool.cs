using System;
using UnityEngine;

namespace Project.Content.ObjectPool
{
    public interface IFiltrablePool<T>
    {
        public T GetByFilter(IPoolFilterStrategy<T> filterStrategy);
        public T GetByFilter(IPoolFilterStrategy<T> filterStrategy, Vector3 position);
        public void Add(T createdObject);
    }
}