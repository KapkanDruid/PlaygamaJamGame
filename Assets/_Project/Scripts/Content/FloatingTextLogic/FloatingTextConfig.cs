using DG.Tweening;
using UnityEngine;

namespace Project.Content
{
    [CreateAssetMenu(fileName = "FloatingTextConfig", menuName = "_Project/FloatingTextConfig")]
    public class FloatingTextConfig : ScriptableObject
    {
        [SerializeField] private float _startScale;
        [SerializeField] private float _scaleMultiplier;
        [SerializeField] private float _scaleChangeTime;
        [SerializeField] private float _fadeTime;
        [SerializeField] private Vector2 _fadePosition;
        [SerializeField] private Ease _showEase;
        [SerializeField] private Ease _fadeEase;

        public float ScaleMultiplier => _scaleMultiplier;
        public float ScaleChangeTime => _scaleChangeTime; 
        public float FadeTime => _fadeTime;
        public Vector2 FadePosition => _fadePosition;
        public Ease ShowEase => _showEase;
        public Ease FadeEase => _fadeEase;
        public float StartScale => _startScale;
    }
}
