using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Project.Content.CharacterAI
{
    public static class RemoteJsonLoader
    {
        public static async UniTask LoadJsonAsync<T>(string url, Action<T> onSuccess, Action<string> onError = null, CancellationToken cancellationToken = default) where T : class
        {
            using var request = UnityWebRequest.Get(url);
            var operation = await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var data = JsonUtility.FromJson<T>(request.downloadHandler.text);
                    onSuccess?.Invoke(data);
                }
                catch (Exception ex)
                {
                    onError?.Invoke($"JSON parse error: {ex.Message}");
                }
            }
            else
            {
                onError?.Invoke($"Request failed: {request.error}");
            }
        }
    }
}

