using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class WinView : MonoBehaviour
    {
        [Header("Win Menu")]
        [SerializeField] private float _timeAnimationButtons = 0.5f;
        [SerializeField] private float _timeAnimationBackground = 0.5f;
        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private GameObject _panel;
        [SerializeField] private RectTransform _background;

        private WinLoseHandler _winLoseHandler;
        private Button[] _buttons;

        [Inject]
        public void Construct(WinLoseHandler winLoseHandler)
        {
            _winLoseHandler = winLoseHandler;
        }

        private void Start()
        {
            _winLoseHandler.OnWin += ShowVictoryMenu;

            _background.gameObject.SetActive(false);
            _buttonsContainer.SetActive(false);
            _panel.SetActive(false);
            _background.transform.localScale = Vector3.zero;

            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();
            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }
        }

        private void ShowVictoryMenu()
        {
            _background.gameObject.SetActive(true);
            _panel.SetActive(true);

            _background.transform.DOScale(1f, _timeAnimationBackground)
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
            _winLoseHandler.OnWin -= ShowVictoryMenu;
        }
    }
}
