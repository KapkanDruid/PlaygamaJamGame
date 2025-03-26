using UnityEngine;

namespace Project.Content
{
    [CreateAssetMenu(fileName = "NameSceneConfig", menuName = "_Project/NameSceneConfig")]
    public class NameSceneConfig : ScriptableObject
    {
        [SerializeField] private string _sceneName;

        public string SceneName => _sceneName;
    }
}
