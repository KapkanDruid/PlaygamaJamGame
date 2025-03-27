using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Project.Content
{
    public class SoundSettingsPopup : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;

        private static float _musicValue = 1;
        private static float _effectsValue = 1;

        private void Start()
        {
            _musicSlider.value = _musicValue;
            _effectsSlider.value = _effectsValue;

            _musicSlider.onValueChanged.AddListener((x) => SetMusicVolume(x));
            _effectsSlider.onValueChanged.AddListener((x) => SetEffectsVolume(x));
        }

        public void SetEffectsVolume(float value)
        {
            _effectsValue = value;
            _audioMixer.SetFloat("SFXValue", Mathf.Log10(value) * 20);
        }

        public void SetMusicVolume(float value)
        {
            _musicValue = value;
            _audioMixer.SetFloat("MusicValue", Mathf.Log10(value) * 20);
        }

        private void OnDestroy()
        {
            _musicSlider.onValueChanged.RemoveAllListeners();
            _effectsSlider.onValueChanged.RemoveAllListeners();
        }
    }
}
