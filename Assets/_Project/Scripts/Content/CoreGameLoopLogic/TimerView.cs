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

        [Inject]
        public void Construct(SceneData sceneData, PauseHandler pauseHandler)
        {
            _sceneData = sceneData;
            _pauseHandler = pauseHandler;
        }

        private void Start()
        {
            _timeRemaining = _sceneData.TimeToWin;
        }

        private void Update()
        {
            if (_pauseHandler.IsPaused)
                return;

            UITimer();
        }

        private void UITimer()
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                if (_timeRemaining < 0)
                    _timeRemaining = 0;

                UpdateTimerDisplay();
            }
        }
        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);
            string formattedTime = string.Join(" ", $"{minutes:D2}:{seconds:D2}".ToCharArray());
            _timerText.text = formattedTime;
        }
    }

}
