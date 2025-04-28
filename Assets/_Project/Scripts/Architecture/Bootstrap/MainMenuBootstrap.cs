using Project.Content;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [Inject] private AudioController _audioController;
        [Inject] private InputSystemActions _inputActions;

        private void Awake()
        {
            _inputActions.Enable();
            _audioController.Initialize();    
        }
    }
}
