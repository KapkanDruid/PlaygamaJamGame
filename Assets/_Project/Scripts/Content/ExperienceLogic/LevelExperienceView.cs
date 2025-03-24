using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Content
{
    public class LevelExperienceView : MonoBehaviour
    {
        [SerializeField] private Image _fillBar;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private RectTransform _hopper;
        [SerializeField] private Transform _disappearPoint;
        [SerializeField] private Transform _preDisappearPoint;
        [SerializeField] private float _disappearSpeed;
        [SerializeField] private float _preDisappearSpeed;
        [SerializeField] private Ease _disappearEase;
        [SerializeField] private Ease _preDisappearEase;

        private List<Sequence> _activeSequences = new();

        public void SetExperienceBar(float currentValue, float maxValue, int currentLevel)
        {
            _fillBar.fillAmount = currentValue / maxValue;
            _levelText.text = currentLevel.ToString();
        }

        public void ShowColletObject(Transform transform, Action onAnimationComplete)
        {
            var sequence = DOTween.Sequence()
                .Append(transform.DOMove(_preDisappearPoint.position, _preDisappearSpeed).SetEase(_preDisappearEase))
                .Append(transform.DOMove(_disappearPoint.position, _disappearSpeed).SetEase(_disappearEase))
                .AppendCallback(() => 
                { 
                    transform.gameObject.SetActive(false);
                    onAnimationComplete?.Invoke();
                });

            RegisterSequenceToPause(sequence);
        }

        private void RegisterSequenceToPause(Sequence sequence)
        {
            _activeSequences.Add(sequence);
            sequence.OnComplete(() => _activeSequences.Remove(sequence));
        }

        public void Pause()
        {
            foreach (var seq in _activeSequences)
            {
                if (seq.IsActive() && seq.IsPlaying())
                    seq.Pause();
            }
        }

        public void Play()
        {
            foreach (var seq in _activeSequences)
            {
                if (seq.IsActive() && !seq.IsPlaying())
                    seq.Play();
            }
        }
    }
}
