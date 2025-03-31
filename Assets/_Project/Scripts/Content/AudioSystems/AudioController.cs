using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _SFXSource;

        [SerializeField] private EffectType _musicListType;

        [SerializeField] private int _startSoundIndex = 0;

        private Sound[] _musicClips;

        private SceneRecourses _sceneResources;

        private int _currentMusicIndex;
        private bool _isStop;

        [Inject]
        private void Construct(SceneRecourses sceneResources)
        {
            _sceneResources = sceneResources;
        }

        public void Initialize()
        {
            _currentMusicIndex = _startSoundIndex;
            _musicSource.loop = false;
            foreach (var musicList in _sceneResources.MusicDictionary)
            {
                if (musicList.Key == _musicListType)
                {
                    _musicClips = musicList.Value;
                    return;
                }
            }

            Debug.LogError("[AudioController] Failed to find Music List");
        }


        public void PlayOneShot(EffectType effectType)
        {
            for (int i = 0; i < _sceneResources.SoundEffects.Length; i++)
            {
                var clip = _sceneResources.SoundEffects[i];

                if (clip.Key == effectType)
                {
                    _SFXSource.PlayOneShot(clip.Value.Clip, clip.Value.Volume);
                    return;
                }
            }

            Debug.LogError("[AudioController] Failed to find sound effect");
        }

        public void PLayLoopEffect(EffectType effectType)
        {
            for (int i = 0; i < _sceneResources.SoundEffects.Length; i++)
            {
                var clip = _sceneResources.SoundEffects[i];

                if (clip.Key == effectType)
                {
                    _SFXSource.loop = true;
                    _SFXSource.clip = clip.Value.Clip;
                    //Добавить множитель звука
                    _SFXSource.Play();

                    return;
                }
            }

            Debug.LogError("[AudioController] Failed to find sound effect");
        }

        public void StopLoopEffect()
        {
            _SFXSource.loop = false;
            _SFXSource.Stop();
        }

        public void StopMusic()
        {
            _isStop = true;
            _musicSource.Stop();
        }

        private void Update()
        {
            if (_isStop)
                return;

            if (!_musicSource.isPlaying)
            {
                PlayNextClip();
            }
        }

        private void PlayNextClip()
        {
            if (_musicClips == null)
                return;

            _musicSource.clip = _musicClips[_currentMusicIndex].Clip;

            _musicSource.volume = _musicClips[_currentMusicIndex].Volume;

            _musicSource.Play();

            _currentMusicIndex++;

            if (_currentMusicIndex >= _musicClips.Length)
                _currentMusicIndex = 0;
        }
    }
}