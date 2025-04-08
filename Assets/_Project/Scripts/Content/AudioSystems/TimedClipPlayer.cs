using Project.Content.CoreGameLoopLogic;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content
{
    public partial class TimedClipPlayer : ITickable
    {
        private List<TimedClip> _timedClips = new();

        private WinLoseHandler _winLoseHandler;
        private AudioController _audioController;

        private float _timerStartValue;

        public TimedClipPlayer(WinLoseHandler winLoseHandler, AudioController audioController, SceneData sceneData)
        {
            _winLoseHandler = winLoseHandler;
            _audioController = audioController;

            _timedClips.AddRange(sceneData.MusicByTime);
            _timerStartValue = _winLoseHandler.TimeToWin;
        }

        public void Tick()
        {
            TimedClip lastClip = null;

            for (int i = 0; i < _timedClips.Count; i++)
            {
                TimedClip clip = _timedClips[i];

                if (_winLoseHandler.TimeToWin <= _timerStartValue - clip.PlayTimeInSeconds)
                {
                    _audioController.PlayMusic(clip.EffectType);
                    lastClip = clip;
                }
            }

            if (lastClip != null)
            {
                _timedClips.Remove(lastClip);
            }
        }
    }
}