using System;
using UnityEngine;

namespace Content
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
