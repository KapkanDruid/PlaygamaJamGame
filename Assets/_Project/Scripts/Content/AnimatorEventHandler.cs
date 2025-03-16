using System;
using UnityEngine;

namespace Project.Content
{
    public class AnimatorEventHandler : MonoBehaviour
    {
        public event Action OnAnimationHit;

        public void HitEventHandle()
        {
            OnAnimationHit?.Invoke();
        }
    }
}
