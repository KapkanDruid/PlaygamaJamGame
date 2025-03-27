using UnityEngine;
using Zenject;

namespace Project.Content.Audio
{
    public class StartEffect : MonoBehaviour
    {
        [SerializeField] private EffectType _effectType;

        private AudioController _audioController;

        [Inject]
        private void Construct(AudioController audioController)
        {
            _audioController = audioController;
        }

        private void Start()
        {
            _audioController.PlayOneShot(_effectType);
        }
    }
}
