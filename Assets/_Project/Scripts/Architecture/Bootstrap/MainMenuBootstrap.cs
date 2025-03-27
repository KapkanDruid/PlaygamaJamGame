using Project.Content;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [Inject] private AudioController _audioController;

        private void Awake()
        {
            _audioController.Initialize();    
        }
    }
}
