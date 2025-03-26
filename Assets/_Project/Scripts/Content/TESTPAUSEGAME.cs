using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content
{
    public class TESTPAUSEGAME : MonoBehaviour
    {
        private PauseHandler _pauseHandler;
        private bool _paused;

        [Inject]
        public void Construct(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;
        }

        public void PauseGame(Toggle paused)
        {
            Debug.Log("Toggle Clicked! Paused: " + paused.isOn);
            _pauseHandler.SetPaused(paused.isOn);
        }

        public void SetPaused()
        {
            _paused = !_paused;
            _pauseHandler.SetPaused(_paused);
        }
    }
}
