using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Project.Content
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private FloatingTextConfig _config;
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private RectTransform _rectTransform;

        private Sequence _tweenSequence;

        public FloatingTextConfig Config => _config; 

        public void Prepare(Vector2 position, string massage)
        {
            _textMeshPro.rectTransform.position = position;
            _textMeshPro.text = massage;

            _rectTransform.localScale = Vector3.one * _config.StartScale;

            Color newColor = _textMeshPro.color;
            newColor.a = 1f;
            _textMeshPro.color = newColor;

            _tweenSequence = DOTween.Sequence()
                .Append(_rectTransform.DOScale(_rectTransform.localScale * _config.ScaleMultiplier, _config.ScaleChangeTime))
                .Append(_textMeshPro.DOFade(0, _config.FadeTime))
                .Join(_rectTransform.DOMove(_rectTransform.position + (Vector3)_config.FadePosition, _config.FadeTime))
                .AppendCallback(OnAnimationEnd);
        }

        private void OnAnimationEnd() => gameObject.SetActive(false);

        private void OnDisable() => _tweenSequence?.Kill();

        private void OnDestroy() => _tweenSequence?.Kill();
    }
}
