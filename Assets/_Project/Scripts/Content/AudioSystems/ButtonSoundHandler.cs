using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Project.Content
{
    public class ButtonSoundHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private EffectType _onClickSound;
        [SerializeField] private EffectType _onEnterSound;
        [SerializeField] private EffectType _onExitSound;

        private AudioController _audioController;

        [Inject]
        private void Construct(AudioController audioController)
        {
            _audioController = audioController;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_onClickSound == null)
                return;

            _audioController.PlayOneShot(_onClickSound);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_onEnterSound == null)
                return;

            _audioController.PlayOneShot(_onEnterSound);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_onExitSound == null)
                return;

            _audioController.PlayOneShot(_onExitSound);
        }
    }
}