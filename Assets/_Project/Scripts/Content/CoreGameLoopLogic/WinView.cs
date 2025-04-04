using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content.CoreGameLoopLogic
{
    public class WinView : MonoBehaviour
    {
        [Header("Win Menu")]
        [SerializeField] private float _backgroundShowSpeed = 0.5f;
        [SerializeField] private float _sequenceIntervalTime = 0.5f;
        [SerializeField] private float _textShowSpeed = 0.2f;
        [SerializeField] private float _textShowInterval = 0.1f;
        [SerializeField] private float _buttonsShowSpeed = 0.5f;

        [Header("Animation Settings"), Space(3f)]
        [SerializeField] private RectTransform _topText;
        [SerializeField] private RectTransform _bottomText;
        [SerializeField] private RectTransform _topTextAnchor;
        [SerializeField] private RectTransform _bottomTextAnchor;
        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private GameObject _anticlicker;
        [SerializeField] private RectTransform _background;
        [SerializeField] private EffectType _winEffect;
        [SerializeField] private Ease _backgroundEase;
        [SerializeField] private Ease _textEase;


        private WinLoseHandler _winLoseHandler;
        private AudioController _audioController;
        private Button[] _buttons;

        [Inject]
        public void Construct(WinLoseHandler winLoseHandler, AudioController audioController)
        {
            _winLoseHandler = winLoseHandler;
            _audioController = audioController;
        }

        private void Start()
        {
            _winLoseHandler.OnWin += ShowVictoryMenu;

            _background.gameObject.SetActive(false);
            _buttonsContainer.SetActive(false);
            _topText.gameObject.SetActive(false);
            _bottomText.gameObject.SetActive(false);
            _anticlicker.gameObject.SetActive(false);

            _buttons = _buttonsContainer.GetComponentsInChildren<Button>();

            foreach (var button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }

            _topText.position = SwitchWithAnchorPosition(_topText, _topTextAnchor);
            _bottomText.position = SwitchWithAnchorPosition(_bottomText, _bottomTextAnchor);
        }

        private void ShowVictoryMenu()
        {
            _anticlicker.gameObject.SetActive(true);

            _audioController.StopMusic();
            _audioController.PlayOneShot(_winEffect);

            _background.gameObject.SetActive(true);

            _topText.gameObject.SetActive(true);
            _bottomText.gameObject.SetActive(true);

            DOTween.Sequence()
                .Append(_background.DOAnchorPos(Vector2.zero, _backgroundShowSpeed).SetEase(_backgroundEase))
                .AppendInterval(_sequenceIntervalTime)
                .Append(_topText.DOMove(SwitchWithAnchorPosition(_topText, _topTextAnchor), _textShowSpeed).SetEase(_textEase))
                .AppendInterval(_textShowInterval)
                .Append(_bottomText.DOMove(SwitchWithAnchorPosition(_bottomText, _bottomTextAnchor), _textShowSpeed).SetEase(_textEase))
                .AppendInterval(_sequenceIntervalTime)
                .OnComplete(() => ShowButtons());
        }

        private Vector2 SwitchWithAnchorPosition(RectTransform master, RectTransform anchor)
        {
            var anchorPosition = anchor.position;
            anchor.position = master.position;

            return anchorPosition;
        }

        private void ShowButtons()
        {
            _buttonsContainer.SetActive(true);

            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].transform.DOScale(1f, _buttonsShowSpeed)
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
