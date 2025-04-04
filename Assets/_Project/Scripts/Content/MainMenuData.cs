using UnityEngine;

namespace Project.Content
{
    public class MainMenuData : MonoBehaviour, IAudioControllerData
    {
        [Header("Music Settings")]
        [SerializeField] private MusicType _musicListType;
        [SerializeField] private int _startMusicIndex = 0;

        public int StartMusicIndex => _startMusicIndex;
        public MusicType MusicListType => _musicListType;
    }
}
