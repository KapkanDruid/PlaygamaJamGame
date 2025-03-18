using UnityEngine;

namespace Project.Content
{
    [CreateAssetMenu(fileName = "SceneRecourses", menuName = "_Project/SceneRecourses")]
    public class SceneRecourses : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private PrefabReferences _prefabs;

        public PrefabReferences Prefabs => _prefabs; 
    }
}