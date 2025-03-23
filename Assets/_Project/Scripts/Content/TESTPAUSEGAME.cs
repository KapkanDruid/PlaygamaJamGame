using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content
{
    public class TESTPAUSEGAME : MonoBehaviour
    {
        private PauseHandler _pauseHandler;

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
    }
}
