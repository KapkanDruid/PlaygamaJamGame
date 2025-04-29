using System;

namespace Project.Content.ObjectPool
{
    public class FilterByPredicate<T> : IPoolFilterStrategy<T>
    {
        private Predicate<T> _predicate;

        public FilterByPredicate(Predicate<T> predicate)
        {
            _predicate = predicate;
        }

        public T Select(T[] objectsToCheck)
        {
            for (int i = 0; i < objectsToCheck.Length; i++)
            {
                T item = objectsToCheck[i];

                if (_predicate(item))
                    return item;
            }

            return default;
        }
    }
}