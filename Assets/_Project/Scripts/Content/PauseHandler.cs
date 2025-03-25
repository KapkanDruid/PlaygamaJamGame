using System;

namespace Project.Content
{
    public class PauseHandler : IPauseHandler
    {
        public event Action<bool> OnPauseChanged;
        public bool IsPaused { get; private set; }
        public void SetPaused(bool isPaused)
        {
            IsPaused = isPaused;
            OnPauseChanged?.Invoke(IsPaused);
        }
    }
}

