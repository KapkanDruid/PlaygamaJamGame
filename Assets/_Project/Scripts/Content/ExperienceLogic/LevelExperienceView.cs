using DG.Tweening;
using Project.Content.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

        private CardsPopupView _cardsView;

        private List<Sequence> _activeSequences = new();

        private bool _isPaused;

        [Inject]
        private void Construct(CardsPopupView cardsView)
        {
            _cardsView = cardsView;

            _cardsView.OnPopupStartShow += Pause;
            _cardsView.OnPopupClosed += Play;
        }

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

            if (_isPaused)
                Pause();
        }

        private void RegisterSequenceToPause(Sequence sequence)
        {
            _activeSequences.Add(sequence);
            sequence.OnComplete(() => _activeSequences.Remove(sequence));
        }

        private void Pause()
        {
            _isPaused = true;
            foreach (var sequence in _activeSequences)
            {
                if (sequence.IsActive() && sequence.IsPlaying())
                    sequence.Pause();
            }
        }

        private void Play()
        {
            _isPaused = false;
            foreach (var sequence in _activeSequences)
            {
                if (sequence.IsActive() && !sequence.IsPlaying())
                    sequence.Play();
            }
        }

        private void OnDestroy()
        {
            if (_cardsView == null)
                return;

            _cardsView.OnPopupStartShow -= Pause;
            _cardsView.OnPopupClosed -= Play;
        }
    }
}
