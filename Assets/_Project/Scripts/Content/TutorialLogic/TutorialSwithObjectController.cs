using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;
using TMPro;
using Project.Content.CoreGameLoopLogic;
using Cysharp.Threading.Tasks;
using Project.Architecture;
using UnityEngine.InputSystem;

namespace Project.Content
{
    public class TutorialSwithObjectController : MonoBehaviour, ISkipHandlerData
    {
        public static event Action OnTutorialFinished;

        [SerializeField] private TutorialObjectData[] _tutorialObjectsData;

        [SerializeField] private TextMeshProUGUI _nameDescriptionText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private RectTransform _uiMask;
        [SerializeField] private RectTransform _marker;
        [SerializeField] private Image _blackImage;

        [SerializeField] private GameObject _tutorialPanel;

        [SerializeField] private Image _radialSlider;
        [SerializeField] private float _fillSpeed = 1f;

        private bool _isPressed;
        private bool _isStarTutorial = true;

        private int _currentIndex = 0;

        private PauseHandler _pauseHandler;

        private Camera _mainCamera;

        private SkipHandler _skipHandler;

        private InputSystemActions _inputSystemActions;

        public Image SkipFiller => _radialSlider;

        public float SkipDuration => _fillSpeed;

        [Inject]
        private void Construct(PauseHandler pauseHandler, Camera mainCamera, SkipHandler skipHandler, InputSystemActions inputActions)
        {
            _pauseHandler = pauseHandler;
            _mainCamera = mainCamera;
            _skipHandler = skipHandler;
            _inputSystemActions = inputActions;

            _inputSystemActions.UI.Skip.performed += context => _isPressed = true;
        }

        private void Start()
        {
            foreach (var tutorialObjectData in _tutorialObjectsData)
            {
                tutorialObjectData.Initialize();
            }

            if (_uiMask != null)
            {
                _uiMask.gameObject.SetActive(false);
            }
            /*  if (_marker != null)
              {
                  _marker.gameObject.SetActive(false);
              }*/
            if (_nameDescriptionText != null)
            {
                _nameDescriptionText.gameObject.SetActive(false);
            }
            if (_descriptionText != null)
            {
                _descriptionText.gameObject.SetActive(false);
            }

            _pauseHandler.SetPaused(true);

            _skipHandler.Initialize(this, this.GetCancellationTokenOnDestroy());

            _skipHandler.IsActive = true;

            _marker.position = new Vector3(1150f, 0f, 0f);
        }

        private void Update()
        {
            if (_isPressed && _tutorialPanel.activeSelf)
            {
                if (_isStarTutorial)
                {
                    StartScreenDisable();
                    
                }
                else
                {
                    SwitchToNextItem();
                    _isPressed = false;
                }
            }

            if (_skipHandler.IsForceSkip)
            {
                ClosePanel();
                _skipHandler.IsActive = false;
            }
        }

        private void SwitchToNextItem()
        {
            if (_currentIndex < _tutorialObjectsData.Length)
            {
                MoveMaskToTransform();
                MoveMarkerToTransform();
                ShowTutorialDescriptionName();
                ShowTutorialDescription();
                _currentIndex++;
            }
            else
            {
                ClosePanel();
            }
        }
        private void MoveMaskToTransform()
        {
            Transform targetObject = _tutorialObjectsData[_currentIndex].ObjectPosition;

            if (targetObject is RectTransform rectTransform)
            {
                Vector3 offset = _tutorialObjectsData[_currentIndex].MaskPositionOffset;

                _uiMask.position = rectTransform.position + offset;

                Vector2 newSize = _tutorialObjectsData[_currentIndex].MaskScale;

                _uiMask.sizeDelta = newSize;

                if (!_uiMask.gameObject.activeInHierarchy)
                {
                    _uiMask.gameObject.SetActive(true);
                }
            }
            else
            {
                Vector3 screenPosition = _mainCamera.WorldToScreenPoint(targetObject.transform.position);

                Vector3 offset = _tutorialObjectsData[_currentIndex].MaskPositionOffset;
                _uiMask.position = screenPosition + offset;

                Vector2 newSize = _tutorialObjectsData[_currentIndex].MaskScale;
                _uiMask.sizeDelta = newSize;

                if (!_uiMask.gameObject.activeInHierarchy)
                {
                    _uiMask.gameObject.SetActive(true);
                }
            }
        }
        private void MoveMarkerToTransform()
        {
            Transform targetObject = _tutorialObjectsData[_currentIndex].ObjectPosition;

            if (targetObject is RectTransform rectTransform)
            {

                Vector3 offset = _tutorialObjectsData[_currentIndex].MarkerPositionOffset;

                _marker.position = rectTransform.position + offset;

                if (!_marker.gameObject.activeInHierarchy)
                {
                    _marker.gameObject.SetActive(true);
                }
            }
            else
            {
                Vector3 screenPosition = _mainCamera.WorldToScreenPoint(targetObject.transform.position);

                Vector3 offset = _tutorialObjectsData[_currentIndex].MarkerPositionOffset;

                _marker.position = screenPosition + offset;

                if (!_marker.gameObject.activeInHierarchy)
                {
                    _marker.gameObject.SetActive(true);
                }
            }
        }

        private void ShowTutorialDescriptionName()
        {
            _nameDescriptionText.text = _tutorialObjectsData[_currentIndex].ObjectName;

            if (!_nameDescriptionText.gameObject.activeInHierarchy)
            {
                _nameDescriptionText.gameObject.SetActive(true);
            }
        }

        private void ShowTutorialDescription()
        {
            _descriptionText.text = _tutorialObjectsData[_currentIndex].ObjectDescription;

            if (!_descriptionText.gameObject.activeInHierarchy)
            {
                _descriptionText.gameObject.SetActive(true);
            }
        }

        private void StartScreenDisable()
        {
            _blackImage.gameObject.SetActive(false);
            _isStarTutorial = false;
        }

        private void OnDestroy()
        {
            foreach (var tutorialObjectData in _tutorialObjectsData)
            {
                tutorialObjectData.Dispose();
            }
        }

        private void ClosePanel()
        {
            _tutorialPanel.SetActive(false);
            OnTutorialFinished?.Invoke();

            _pauseHandler.SetPaused(false);
        }
    }
}



