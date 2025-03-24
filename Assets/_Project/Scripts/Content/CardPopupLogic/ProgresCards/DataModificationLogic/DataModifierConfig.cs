using Project.Content.UI;
using UnityEngine;

namespace Project.Content.UI.DataModification
{
    public abstract class DataModifierConfig : ScriptableObject, ICoreProgressStrategy
    {
        protected SceneData SceneData;

        public void SetSceneData(SceneData sceneData)
        {
            SceneData = sceneData;
        }
        public abstract void ExecuteProgress();
    }
}
