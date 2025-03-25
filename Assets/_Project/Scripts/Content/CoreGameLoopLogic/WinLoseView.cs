using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class WinLoseView : MonoBehaviour 
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private float _timeAnimationButtons = 1.5f;
        [SerializeField] private GameObject _buttonsContainer;

        [Header("Win Menu")]
        [SerializeField] private RectTransform _background; 
        [SerializeField] private float _winTimeAnimationBackground = 1.5f;


        [Header("Lose Menu")]
        [SerializeField] private RectTransform _circle; 
        [SerializeField] private Vector2 _centerPosition; 
        [SerializeField] private float _moveDuration = 3f;
        [SerializeField] private float _rotationAngle = 360f;

        private WinLoseHandler _winLoseHandler;
        private SceneData _sceneData;
        private Button[] _buttons;
        private float _timeRemaining;

        [Inject]
        public void Construct(WinLoseHandler winLoseHandler, SceneData sceneData)
        {
            _winLoseHandler = winLoseHandler;
            _sceneData = sceneData;
        }

        private void Start()
        {
            _winLoseHandler.OnWin += ShowVictoryMenu;
            _winLoseHandler.OnLose += ShowDefeatedMenu;

            _circle.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
            _buttonsContainer.SetActive(false);
            _background.transform.localScale = Vector3.zero;

            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();
            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }

            _timeRemaining = _sceneData.TimeToWin;
        }

        private void Update()
        {
            UITimer();
        }

        private void UITimer()
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
        }

        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);
            _timerText.text = $"{minutes}:{seconds}";
        }
        private void ShowDefeatedMenu()
        {
            _circle.gameObject.SetActive(true);


            _circle.DOAnchorPos(_centerPosition, _moveDuration).SetEase(Ease.OutQuad);

            _circle.DORotate(new Vector3(0, 0, _rotationAngle), _moveDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad)
                .OnComplete(ShowButtons);
        }

        private void ShowVictoryMenu()
        {
            _background.gameObject.SetActive(true);

            _background.transform.DOScale(1f, _winTimeAnimationBackground)
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
            _winLoseHandler.OnLose -= ShowDefeatedMenu;
        }
    }
}
