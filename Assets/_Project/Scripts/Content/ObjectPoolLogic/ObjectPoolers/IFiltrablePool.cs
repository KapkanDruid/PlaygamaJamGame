using System;

namespace Project.Content.ObjectPool
{
    public interface IFiltrablePool<T>
    {
        public T GetByFilter(IPoolFilterStrategy<T> filterStrategy);
        public void Add(T createdObject);
    }
}