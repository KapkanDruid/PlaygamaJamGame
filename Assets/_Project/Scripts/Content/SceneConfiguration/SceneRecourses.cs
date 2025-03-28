using UnityEngine;

namespace Project.Content
{
    [CreateAssetMenu(fileName = "SceneRecourses", menuName = "_Project/SceneRecourses")]
    public class SceneRecourses : ScriptableObject
    {
        [SerializeField] private PrefabReferences _prefabs;
        [SerializeField] private ConfigReferences _configs;

        [SerializeField] private CustomDictionary<EffectType, Sound>[] _soundEffects;
        [SerializeField] private CustomDictionary<EffectType, Sound[]>[] _music;
        [SerializeField] private CustomDictionary<AlertType, Alert>[] _alerts;

        public PrefabReferences Prefabs => _prefabs;
        public ConfigReferences Configs => _configs;
        public CustomDictionary<EffectType, Sound[]>[] MusicDictionary => _music;
        public CustomDictionary<EffectType, Sound>[] SoundEffects => _soundEffects; 
        public CustomDictionary<AlertType, Alert>[] Alerts => _alerts; 
    }
}