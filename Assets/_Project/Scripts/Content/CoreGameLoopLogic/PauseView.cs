using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private GameObject _background;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Dropdown _languageDropdown;

        private PauseHandler _pauseHandler;
        private Button[] _buttons;


        [Inject]
        public void Construct(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;
        }

        private void Start()
        {
            _pauseButton.onClick.AddListener(ShowPauseMenu);
            _resumeButton.onClick.AddListener(HidePauseMenu);

            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();

            _buttonsContainer.SetActive(false);
            _background.SetActive(false);
            _languageDropdown.gameObject.SetActive(false);
            _resumeButton.gameObject.SetActive(false);
            _panel.SetActive(false);

            _resumeButton.transform.localScale = Vector3.zero;
            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();
            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }

        }

        private void ShowPauseMenu()
        {
            _pauseHandler.SetPaused(true);
            _background.SetActive(true);
            _panel.SetActive(true);
            _resumeButton.gameObject.SetActive(true);
            _languageDropdown.gameObject.SetActive(true);
            _buttonsContainer.SetActive(true);

            _background.transform.localScale = Vector3.zero;
            _background.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);

            _resumeButton.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuad);

            _languageDropdown.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuad);

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].gameObject.SetActive(true);
                _buttons[i].transform.DOScale(1f, 0.5f)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(i * 0.2f);
            }

        }

        private void HidePauseMenu()
        {
            _pauseHandler.SetPaused(false);
            _panel.SetActive(false);

            for (int i = 0; i < _buttons.Length; i++)
            {
                HideButtons(i);
            }

            _resumeButton.transform.DOScale(0f, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() => _resumeButton.gameObject.SetActive(false));

            _languageDropdown.transform.DOScale(0f, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() => _languageDropdown.gameObject.SetActive(false));

            _background.transform.DOScale(0f, 0.5f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _background.SetActive(false);
                    _buttonsContainer.SetActive(false);
                });
        }

        private void HideButtons(int i)
        {
            _buttons[i].transform.DOScale(0f, 0.3f)
                                .SetEase(Ease.InBack)
                                .SetDelay((_buttons.Length - 1 - i) * 0.1f)
                                .OnComplete(() => _buttons[i].gameObject.SetActive(false));
        }
    }
}
