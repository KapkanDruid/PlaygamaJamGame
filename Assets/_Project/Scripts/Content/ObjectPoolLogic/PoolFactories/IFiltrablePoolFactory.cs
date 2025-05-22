using Cysharp.Threading.Tasks;
using System;

namespace Project.Content.ObjectPool
{
    public interface IFiltrablePoolFactory
    {
        public Type PoolType { get; }
        public object Create();
        UniTask PreloadAsync();
        void Release();
    }
}