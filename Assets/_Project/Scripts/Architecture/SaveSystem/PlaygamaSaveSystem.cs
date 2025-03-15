using Playgama;
using System;
using UnityEngine;
using Newtonsoft.Json;

namespace _Project.Architecture.SaveSystem
{
    public class PlaygamaSaveSystem : ISaveSystem
    {
        public void Save<T>(string key, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            Bridge.storage.Set(key, json, OnStorageSetCompleted);
        }

        public void Load<T>(string key, Action<T> onLoaded)
        {
            Bridge.storage.Get(key, (success, data) => OnStorageGetCompleted(success, data, onLoaded));
        }

        private void OnStorageGetCompleted<T>(bool success, string data, Action<T> onLoaded)
        {
            if (success && !string.IsNullOrEmpty(data))
            {
                try
                {
                    T deserializedData = JsonConvert.DeserializeObject<T>(data);
                    onLoaded?.Invoke(deserializedData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка десериализации данных: {ex.Message}");
                    onLoaded?.Invoke(default);
                }
            }
            else
            {
                Debug.LogWarning($"Не удалось загрузить данные по ключу: {data}");
                onLoaded?.Invoke(default);
            }
        }

        private void OnStorageSetCompleted(bool success)
        {
            if (!success)
            {
                Debug.LogError("Ошибка сохранения данных");
            }
        }

    }
}
