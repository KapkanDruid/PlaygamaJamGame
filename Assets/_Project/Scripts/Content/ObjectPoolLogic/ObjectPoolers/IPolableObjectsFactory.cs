using Project.Content.ObjectPool;

namespace Project.Content
{
    public interface IPolableObjectsFactory<T>
    {
        public T CreateByFilter(IPoolFilterStrategy<T> filter);
    }
}
