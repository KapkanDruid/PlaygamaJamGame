﻿using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Project.Content
{
    public class AlertController : MonoBehaviour 
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RectTransform _banner;
        [SerializeField] private float _moveDuration = 3f;
        [SerializeField] private float _displayDuration = 2f;
        [SerializeField] private RectTransform _endPositionTransform;

        private SceneRecourses _sceneResources;
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private bool _isAnimating;

        [Inject]
        private void Construct(SceneRecourses sceneResources)
        {
            _sceneResources = sceneResources;
        }

        private void Start()
        {
            _startPosition = _banner.anchoredPosition;
            _endPosition = _endPositionTransform.anchoredPosition;
            _text.gameObject.SetActive(false);
            _isAnimating = false;
        }

        public void ShowAlert(AlertType alertType)
        {
            if (_isAnimating) return;

            for (int i = 0; i < _sceneResources.Alerts.Length; i++)
            {
                var alert = _sceneResources.Alerts[i];

                if (alert.Key == alertType)
                {
                    LocalizeAlert(alert.Value.Key);
                    return;
                }
            }

            Debug.LogError("[AlertController] Failed to find alert text");
        }

        private void LocalizeAlert(string key)
        {
            var localizedString = new LocalizedString { TableReference = "Alert", TableEntryReference = key };
            localizedString.StringChanged += UpdateAlertText;
            localizedString.RefreshString();
        }

        private void UpdateAlertText(string localizedText)
        {
            if (string.IsNullOrEmpty(localizedText))
            {
                Debug.LogError("[AlertController] Failed to localize text for key");
                return;
            }

            _text.text = localizedText;
            AnimateBanner();
        }

        private void AnimateBanner()
        {
            _isAnimating = true;
            _banner.DOAnchorPos(_endPosition, _moveDuration).OnComplete(() =>
            {
                _text.gameObject.SetActive(true);
                DOVirtual.DelayedCall(_displayDuration, () =>
                {
                    _text.gameObject.SetActive(false);
                    _banner.DOAnchorPos(_startPosition, _moveDuration).OnComplete(() =>
                    {
                        _isAnimating = false;
                    });
                });
            });
        }

    }
}
