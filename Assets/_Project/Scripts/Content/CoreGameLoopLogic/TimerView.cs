using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        private float _timeRemaining;
        private SceneData _sceneData;
        private PauseHandler _pauseHandler;
        private WinLoseHandler _winLoseHandler;

        [Inject]
        public void Construct(SceneData sceneData, PauseHandler pauseHandler, WinLoseHandler winLoseHandler)
        {
            _sceneData = sceneData;
            _pauseHandler = pauseHandler;
            _winLoseHandler = winLoseHandler;
        }

        private void Start()
        {
            _timeRemaining = _sceneData.TimeToWin;
        }

        private void Update()
        {
            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(_winLoseHandler.TimeToWin / 60);
            int seconds = Mathf.FloorToInt(_winLoseHandler.TimeToWin % 60);
            string formattedTime = string.Join(" ", $"{minutes:D2}:{seconds:D2}".ToCharArray());
            _timerText.text = formattedTime;
        }
    }

}
