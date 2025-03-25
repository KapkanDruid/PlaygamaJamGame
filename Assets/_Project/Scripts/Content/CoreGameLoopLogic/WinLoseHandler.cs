using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class WinLoseHandler : ITickable
    {
        private PauseHandler _pauseHandler;
        private SceneData _sceneData;
        private float _timeToWin;

        public event Action OnWin;
        public event Action OnLose;

        public WinLoseHandler(PauseHandler pauseHandler, SceneData sceneData)
        {
            _pauseHandler = pauseHandler;
            _sceneData = sceneData;
            _timeToWin = _sceneData.TimeToWin;
        }

        public void MainBildingDestroyed(bool isDestroyed)
        {
            _pauseHandler.SetPaused(isDestroyed);
            OnLose?.Invoke();

        }

        public void Tick()
        {
            TimerToVictory();
        }

        private void TimerToVictory()
        {
            _timeToWin -= Time.deltaTime;
            if (_timeToWin <= 0)
            {
                OnWin?.Invoke();
                _pauseHandler.SetPaused(true);
            }
        }
    }
}
