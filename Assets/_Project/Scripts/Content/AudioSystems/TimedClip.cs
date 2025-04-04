using System;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class TimedClip
    {
        [SerializeField] private MusicType _effectType;
        [SerializeField] private int _playTimeInSeconds;

        public int PlayTimeInSeconds => _playTimeInSeconds;
        public MusicType EffectType => _effectType;
    }
}