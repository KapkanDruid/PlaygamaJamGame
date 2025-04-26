using System;

namespace Project.Content.ObjectPool
{
    public interface IFiltrablePool<T>
    {
        public T GetByFilter(IPoolFilterStrategy<T> filterStrategy, Func<T> createObject);
        public void Add(T createdObject);
    }
}