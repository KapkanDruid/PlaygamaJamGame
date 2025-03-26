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
        public abstract float GetModifierValue();
        public abstract void ExecuteProgress();
    }
}
