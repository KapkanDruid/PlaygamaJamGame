using System;

namespace Project.Content.ObjectPool
{
    public interface IFiltrablePoolFactory
    {
        public Type PoolType { get; }
        public object Create();
    }
}