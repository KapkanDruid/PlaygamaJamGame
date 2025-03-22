using UnityEngine;

namespace Project.Content
{
    [CreateAssetMenu(fileName = "SceneRecourses", menuName = "_Project/SceneRecourses")]
    public class SceneRecourses : ScriptableObject
    {
        [SerializeField] private PrefabReferences _prefabs;
        [SerializeField] private ConfigReferences _configs;

        public PrefabReferences Prefabs => _prefabs;
        public ConfigReferences Configs => _configs; 
    }
}