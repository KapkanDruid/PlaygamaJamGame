using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Content.UI
{
    public abstract class CoreProgressCard : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform; 

        public abstract Button Button { get; }
        public abstract event Action<ICoreProgressStrategy> OnCardSelected;
    }
}
