using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.ObjectPool
{
    public class FiltrablePoolsHandler
    {
        private readonly Dictionary<Type, object> _pools = new();
        private readonly List<IFiltrablePoolFactory> _poolFactories;

        public FiltrablePoolsHandler(List<IFiltrablePoolFactory> poolFactories) 
        {
            _poolFactories = poolFactories;
        }

        public void Initialize()
        {
            foreach (var factory in _poolFactories)
            {
                var pool = factory.Create();

                _pools.Add(factory.PoolType, pool);
            }
        }

        public void AddPool<T>(IFiltrablePool<T> pool)
        {
            var key = typeof(T);

            if (!_pools.ContainsKey(key))
            {
                _pools.Add(key, pool);
            }
            else
            {
                Debug.LogError($"Object pool for {key.Name} already exists, created prefabs will not be used with the pooling system");
            }
        }

        public T GetByFilter<T>(IPoolFilterStrategy<T> poolFilter)
        {
            var key = typeof(T);

            if (_pools.ContainsKey(key))
            {
                if (_pools[key] is IFiltrablePool<T> pool)
                {
                    return pool.GetByFilter(poolFilter);
                }
            }

            Debug.LogError($"[FiltrablePoolsHandler] Failed to GetFromPoolByFilter, pool of {key.Name} does not exist");
            return default;
        }

        public T GetByPredicate<T>(Predicate<T> predicate)
        {
            var key = typeof(T);

            if (_pools.ContainsKey(key))
            {
                if (_pools[key] is IFiltrablePool<T> pool)
                {
                    return pool.GetByFilter(new FilterByPredicate<T>(predicate));
                }
            }

            Debug.LogError($"[FiltrablePoolsHandler] Failed to GetFromPoolByFilter, pool of {key.Name} does not exist");
            return default;
        }
    }
}