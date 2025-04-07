using UnityEngine.UI;

namespace Project.Content.CoreGameLoopLogic
{
    public interface ISkipHandlerData 
    {
        public Image SkipFiller { get; }
        public float SkipDuration { get; }
    }
}
