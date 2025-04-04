using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _SFXSource;

        private Sound[] _musicClips;

        private IAudioControllerData _data;
        private SceneRecourses _sceneResources;

        private int _currentMusicIndex;
        private bool _isStop;

        [Inject]
        private void Construct(SceneRecourses sceneResources, IAudioControllerData data)
        {
            _sceneResources = sceneResources;
            _data = data;
        }

        public void Initialize()
        {
            _currentMusicIndex = _data.StartMusicIndex;
            _musicSource.loop = false;
            foreach (var musicList in _sceneResources.MusicPlayList)
            {
                if (musicList.Key == _data.MusicListType)
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

                    _SFXSource.volume = clip.Value.Volume;
                    _SFXSource.clip = clip.Value.Clip;
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

        public void PlayMusic(MusicType effectType)
        {
            for (int i = 0; i < _sceneResources.MusicByType.Length; i++)
            {
                var clip = _sceneResources.MusicByType[i];

                if (clip.Key == effectType)
                {
                    _musicSource.volume = clip.Value.Volume;
                    _musicSource.clip = clip.Value.Clip;
                    _musicSource.Play();

                    return;
                }
            }

            Debug.LogError("[AudioController] Failed to find music by type");
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

            if (!_musicSource.isPlaying && _musicSource.time == 0)
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