using System;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class Sound
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField, Range(0,1)] private float _volume;

        public AudioClip Clip => _clip;
        public float Volume => _volume;
    }
}