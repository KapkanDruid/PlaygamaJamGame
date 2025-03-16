using System;

namespace Project.Architecture.SaveSystem
{
    public interface ISaveSystem
    {
        void Save<T>(string key, T data);
        void Load<T>(string key, Action<T> onLoaded);
    }
}