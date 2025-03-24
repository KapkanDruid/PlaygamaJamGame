using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.UI
{
    public class CardsPopupView : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private CardsPopupPresenter _presenter;
        [SerializeField] private RectTransform[] _cardPoints;
        [SerializeField] private float _hideOffset;
        [SerializeField] private float _showInterval;
        [SerializeField] private float _showSpeed;
        [SerializeField] private float _hideSpeed;
        [SerializeField] private Ease _showEase;
        [SerializeField] private Ease _hideEase;

        private RectTransform _localRectTransform;
        private UIPositionMath _popupPositionMath;

        private IReadOnlyList<CoreProgressCard> _currentCards;

        public event Action OnPopupStartShow;
        public event Action OnPopupClosed;

        private bool _isActive;
        private bool _isShowing;

        public void Initialize()
        {
            _presenter.CurrentCards.OnValueChanged += ShowPopup;
            _localRectTransform = GetComponent<RectTransform>();
            _popupPositionMath = new UIPositionMath(_localRectTransform, _canvasRectTransform);
            _localRectTransform.localPosition = _popupPositionMath.DetermineHidePosition(_hideOffset);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _localRectTransform.anchoredPosition = _popupPositionMath.StartPosition;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
                _localRectTransform.anchoredPosition = _popupPositionMath.DetermineHidePosition(_hideOffset);
        }

        public void Show()
        {
            if (_isActive)
                return;

            _isActive = true;

            _presenter.Show();
        }

        private void ShowPopup(IReadOnlyList<CoreProgressCard> cards)
        {
            if (_isShowing)
                return;

            _isShowing = true;

            OnPopupStartShow?.Invoke();

            ShowPopupCardsAsync(cards).Forget();
        }

        private async UniTask ShowPopupCardsAsync(IReadOnlyList<CoreProgressCard> cards)
        {
            _currentCards = cards;

            List<UIPositionMath> positionCalculators = new();

            for (int i = 0; i < _currentCards.Count; i++)
            {
                CoreProgressCard card = _currentCards[i];

                card.Button.interactable = false;
                card.RectTransform.anchoredPosition = _cardPoints[i].anchoredPosition;

                UIPositionMath positionCalculator = new UIPositionMath(card.RectTransform, _canvasRectTransform);

                card.RectTransform.anchoredPosition = positionCalculator.DetermineHidePosition(_hideOffset);

                positionCalculators.Add(positionCalculator);
            }

            _localRectTransform.anchoredPosition = _popupPositionMath.StartPosition;
            gameObject.SetActive(true);

            for (int i = 0; i < _currentCards.Count; i++)
            {
                ShowCard(_currentCards[i], positionCalculators[i]);

                try
                {
                    await UniTask.WaitForSeconds(_showInterval, cancellationToken: this.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }

            try
            {
                await UniTask.WaitForSeconds(_showInterval * (_currentCards.Count - 1), cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException)
            {
                return;
            }

            foreach (var card in _currentCards)
            {
                card.Button.interactable = true;
            }

            _isShowing = false;
        }

        private void ShowCard(CoreProgressCard card, UIPositionMath positionCalculator)
        {
            card.RectTransform
                .DOAnchorPos(positionCalculator.StartPosition, _showSpeed)
                .SetEase(_showEase)
                .OnComplete(() =>
                {
                    card.OnCardSelected += OnCardSelected;
                });
        }

        private void OnCardSelected(ICoreProgressStrategy progressStrategy)
        {
            foreach (var card in _currentCards)
            {
                card.OnCardSelected -= OnCardSelected;
            }

            _currentCards = null;

            _localRectTransform
                .DOAnchorPos(_popupPositionMath.DetermineHidePosition(_hideOffset), _hideSpeed)
                .SetEase(_hideEase)
                .OnComplete(() =>
                {
                    _isActive = false;
                    _presenter.CardSelected(progressStrategy);
                    gameObject.SetActive(false);
                    OnPopupClosed?.Invoke();
                });
        }

        private void OnDestroy()
        {
            _presenter.CurrentCards.OnValueChanged -= ShowPopup;
        }
    }
}
