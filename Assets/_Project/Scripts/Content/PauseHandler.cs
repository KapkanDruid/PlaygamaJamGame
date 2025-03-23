using UnityEngine;

namespace Project.Content
{
    public class PauseHandler : IPauseHandler
    {
        public bool IsPaused { get; private set; }

        public void SetPaused(bool isPaused)
        {
            IsPaused = isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }
    }
}

