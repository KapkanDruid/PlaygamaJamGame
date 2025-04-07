using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Project.Content
{
    public class TutorialSwithObjectController : MonoBehaviour
    {
        public static event Action OnTutorialFinished;

        [SerializeField] private TutorialObjectData[] _tutorialObjectsData;

        [SerializeField] private TextMeshProUGUI _nameDescriptionText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private RectTransform _uiMask;
        [SerializeField] private RectTransform _marker;

        [SerializeField] private GameObject _tutorialPanel;

        [SerializeField] private Image _radialSlider;
        [SerializeField] private float _fillSpeed = 1f;
        private bool isFilling = false;

        private int _currentIndex = 0;

        private PauseHandler _pauseHandler;

        private Camera _mainCamera;

        [Inject]
        private void Construct(PauseHandler pauseHandler, Camera mainCamera)
        {
            _pauseHandler = pauseHandler;
            _mainCamera = mainCamera;
        }

        private void Start()
        {
            if (_uiMask != null)
            {
                _uiMask.gameObject.SetActive(false);
            }
            if (_marker != null)
            {
                _marker.gameObject.SetActive(false);
            }
            if (_nameDescriptionText != null)
            {
                _nameDescriptionText.gameObject.SetActive(false);
            }
            if (_descriptionText != null)
            {
                _descriptionText.gameObject.SetActive(false);
            }

            _pauseHandler.SetPaused(true);

            foreach (var tutorialObjectData in _tutorialObjectsData)
            {
                tutorialObjectData.Initialize();
            }
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _tutorialPanel.activeSelf && !isFilling)
            {
                SwitchToNextItem();
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && _tutorialPanel.activeSelf && !isFilling)
            {
                SwitchToNextItem();
            }

            if (Input.GetKey(KeyCode.Space) && _tutorialPanel.activeSelf)
            {
                isFilling = true;
                _radialSlider.fillAmount += _fillSpeed * Time.deltaTime;

                if (_radialSlider.fillAmount >= 1f)
                {
                    ClosePanel();
                    isFilling = false;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                isFilling = false;
                _radialSlider.fillAmount = 0f;
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



