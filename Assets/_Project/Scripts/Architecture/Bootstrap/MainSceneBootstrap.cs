using System;
using UnityEngine;
using Zenject;

public class MainSceneBootstrap : MonoBehaviour
{

    public event Action OnServicesInitialized;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {

        OnServicesInitialized?.Invoke();
    }

    private void Dispose()
    {

    }

    private void OnDisable()
    {
        Dispose();
    }
}
