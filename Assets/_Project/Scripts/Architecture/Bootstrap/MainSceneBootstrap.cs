using System;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainSceneBootstrap : MonoBehaviour
    {
        [Inject] private InputSystemActions _inputActions;

        public static event Action OnServicesInitialized;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _inputActions.Enable();

            OnServicesInitialized?.Invoke();
        }

        private void Dispose()
        {
            _inputActions.Disable();
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}