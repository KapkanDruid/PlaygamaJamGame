using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class LoseView : MonoBehaviour
    {
        [Header("Lose Menu")]
        [SerializeField] private RectTransform _circle;
        [SerializeField] private Vector2 _centerPosition;
        [SerializeField] private float _moveDuration = 3f;
        [SerializeField] private float _rotationAngle = 360f;
        [SerializeField] private float _timeAnimationButtons = 0.5f;
        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private GameObject _panel;

        private WinLoseHandler _winLoseHandler;
        private Button[] _buttons;

        [Inject]
        public void Construct(WinLoseHandler winLoseHandler)
        {
            _winLoseHandler = winLoseHandler;
        }

        private void Start()
        {
            _winLoseHandler.OnLose += ShowDefeatedMenu;

            _circle.gameObject.SetActive(false);
            _buttonsContainer.SetActive(false);
            _panel.SetActive(false);

            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();
            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }

        }

        private void ShowDefeatedMenu()
        {
            _circle.gameObject.SetActive(true);
            _panel.SetActive(true);

            _circle.DOAnchorPos(_centerPosition, _moveDuration).SetEase(Ease.OutQuad);

            _circle.DORotate(new Vector3(0, 0, _rotationAngle), _moveDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad)
                .OnComplete(ShowButtons);
        }

        private void ShowButtons()
        {
            _buttonsContainer.SetActive(true);

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].transform.DOScale(1f, _timeAnimationButtons)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(i * 0.2f);
            }
        }

        private void OnDestroy()
        {
            _winLoseHandler.OnLose -= ShowDefeatedMenu;
        }
    }
}
