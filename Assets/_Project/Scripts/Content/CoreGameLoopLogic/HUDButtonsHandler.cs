using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Content
{
    public class HUDButtonsHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private GameObject _buttonsContainer;

        private Button[] _buttons;

        private void Start()
        {
            _pauseButton.onClick.AddListener(ShowPauseMenu);
            _resumeButton.onClick.AddListener(HidePauseMenu);

            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();

            _buttonsContainer.SetActive(false);
            _pauseMenu.SetActive(false);
            _resumeButton.gameObject.SetActive(false);

            _resumeButton.transform.localScale = Vector3.zero;
            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();
            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }

        }

        private void ShowPauseMenu()
        {
            _pauseMenu.SetActive(true);
            _pauseMenu.transform.localScale = Vector3.zero;
            _pauseMenu.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);

            _resumeButton.gameObject.SetActive(true);
            _buttonsContainer.SetActive(true);

            _resumeButton.transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuad);

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
            for (int i = 0; i < _buttons.Length; i++)
            {
                HideButtons(i);
            }

            _resumeButton.transform.DOScale(0f, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() => _resumeButton.gameObject.SetActive(false));

            _pauseMenu.transform.DOScale(0f, 0.5f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _pauseMenu.SetActive(false);
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
