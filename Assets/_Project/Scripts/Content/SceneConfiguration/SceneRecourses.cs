using UnityEngine;

namespace Project.Content
{
    [CreateAssetMenu(fileName = "SceneRecourses", menuName = "_Project/SceneRecourses")]
    public class SceneRecourses : ScriptableObject
    {
        [SerializeField] private PrefabReferences _prefabs;
        [SerializeField] private ConfigReferences _configs;

        [SerializeField] private CustomDictionary<EffectType, Sound>[] _soundEffects;
        [SerializeField] private CustomDictionary<MusicType, Sound[]>[] _musicPlayList;
        [SerializeField] private CustomDictionary<AlertType, Alert>[] _alerts;
        [SerializeField] private CustomDictionary<MusicType, Sound>[] _musicByType;

        public PrefabReferences Prefabs => _prefabs;
        public ConfigReferences Configs => _configs;
        public CustomDictionary<MusicType, Sound[]>[] MusicPlayList => _musicPlayList;
        public CustomDictionary<EffectType, Sound>[] SoundEffects => _soundEffects; 
        public CustomDictionary<AlertType, Alert>[] Alerts => _alerts;
        public CustomDictionary<MusicType, Sound>[] MusicByType => _musicByType;
    }
}