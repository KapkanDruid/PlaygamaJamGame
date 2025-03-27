using DG.Tweening;
using Project.Content.UI.DataModification;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Content.UI
{
    public class ModifierIcon : MonoBehaviour
    {
        [SerializeField] private DataModifierConfig _modifierConfig;
        [SerializeField] private TextMeshProUGUI _modifierCountText;
        [SerializeField] private RectTransform _showObject;
        [SerializeField] private float _showSpeed;
        [SerializeField] private float _hideSpeed;
        [SerializeField] private Ease _showEase;
        [SerializeField] private Ease _hideEase;
        [SerializeField] private RectTransform _upPosition;
        [SerializeField] private RectTransform _downPosition;

        private Button _button;
        private Tween _currentTwin;
        private int _modifierCount;
        private bool _isObjectShown;

        private void Start()
        {
            DataModifierCard.OnModifierApplied += ShowIcon;
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() => ShowObject());

            _showObject.anchoredPosition = new Vector2(_showObject.anchoredPosition.x, _downPosition.anchoredPosition.y);

            gameObject.SetActive(false);
            _modifierCountText.gameObject.SetActive(false);
        }

        private void ShowIcon(DataModifierConfig modifierConfig)
        {
            if (modifierConfig != _modifierConfig)
                return;

            _modifierCount++;

            if (_modifierCount < 2)
            {
                gameObject.SetActive(true);
                _modifierCountText.gameObject.SetActive(false);
                return;
            }

            _modifierCountText.gameObject.SetActive(true);
            gameObject.SetActive(true);

            _modifierCountText.text = "x" + _modifierCount.ToString();
        }

        private void ShowObject()
        {
            if (!_isObjectShown)
            {
                _currentTwin.Kill();
                _isObjectShown = true;
                ShowCard();
            }
            else
            {
                _currentTwin.Kill();
                _isObjectShown = false;
                HideCard();
            }
        }

        private void ShowCard()
        {
            _showObject.gameObject.SetActive(true);

            _currentTwin = _showObject
                .DOAnchorPos(new Vector2(_showObject.anchoredPosition.x, _upPosition.anchoredPosition.y), _showSpeed)
                .SetEase(_showEase);
        }

        private void HideCard()
        {
            _currentTwin = _showObject
                .DOAnchorPos(new Vector2(_showObject.anchoredPosition.x, _downPosition.anchoredPosition.y), _hideSpeed)
                .SetEase(_hideEase)
                .OnComplete(() => _showObject.gameObject.SetActive(false));
        }

        private void OnDestroy()
        {
            DataModifierCard.OnModifierApplied -= ShowIcon;
            _button.onClick.RemoveAllListeners();
        }
    }
}
