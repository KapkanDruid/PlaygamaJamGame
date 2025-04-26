namespace Project.Content.ObjectPool
{
    public interface IPoolFilterStrategy<T>
    {
        public T Select(T[] objectsToCheck);
    }
}