using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Zenject;
using System;
using System.Reflection;
using TMPro;

namespace Project.Content
{
    public class TutorialSwithObjectController : MonoBehaviour
    {
        public static event Action OnTutorialFinished;
                
        [SerializeField] private GameObject[] _sceneObjectNames;
        [SerializeField] private RectTransform[] _uiElementNames;
                
        [SerializeField] private List<string> _sceneObjectDescriptionName;
        [SerializeField] private List<string> _uiDescriptionName;
        [SerializeField] private TextMeshProUGUI _descriptionNameText;
        [SerializeField] private List<string> _sceneObjectDescription;
        [SerializeField] private List<string> _uiDescription;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        
        [SerializeField] private RectTransform _uiMask;
        [SerializeField] private RectTransform _marker;
        private int _currentIndex = -1; 
        private bool isSearchingSceneObjects = true;

        [SerializeField] private GameObject _panel;
        [SerializeField] private Image _radialSlider;
        [SerializeField] private float _fillSpeed = 1f;
        private bool isFilling = false;

        [SerializeField] private Vector2[] _maskSize;
        [SerializeField] private Vector3[] _offsets;

        [SerializeField] private Vector3[] _markeroffsets;
        
        private PauseHandler _pauseHandler;

        private enum SelectionMode { SceneObjects, UIElements }
        private SelectionMode currentMode = SelectionMode.SceneObjects;

        [Inject]
        private void Construct(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;
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
            if (_descriptionNameText != null) {
                _descriptionNameText.gameObject.SetActive(false);
            }
            if (_descriptionText != null) { 
            _descriptionText.gameObject.SetActive(false);
            }

            _pauseHandler.SetPaused(true);
        }

        private void Update()
        {            
            if (Input.GetMouseButtonDown(0) && _panel.activeSelf && !isFilling) 
            {
                SwitchToNextItem();
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && _panel.activeSelf && !isFilling) 
            {
                SwitchToNextItem();
            }

            if (Input.GetKey(KeyCode.Space) && _panel.activeSelf)
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
            if (isSearchingSceneObjects)
            {
                if (_currentIndex < _sceneObjectNames.Length - 1)
                {
                    _currentIndex++;
                    MoveToSceneObject(_currentIndex);
                    MarkerMoveToSceneObject(_currentIndex);
                    ShowSceneObjcetDescriptionName(_currentIndex);
                    ShowSceneObjectDescription(_currentIndex);

                }
                else
                {
                    isSearchingSceneObjects = false;
                    _currentIndex = -1;
                }
            }
            else
            {
                if (_currentIndex < _uiElementNames.Length - 1)
                {
                    _currentIndex++;
                    MoveToUIElement(_currentIndex);
                    MarkerMoveToUIElement(_currentIndex);
                    ShowUIDescriptionName(_currentIndex);
                    ShowUIDescription(_currentIndex);
                }
                else
                {
                    ClosePanel();
                }
            }            
        }

        private void MoveToSceneObject(int index)
        {
            GameObject targetObject = _sceneObjectNames[index];

            if (targetObject != null)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetObject.transform.position);
                               
                Vector3 offset = (_currentIndex >= 0 && _currentIndex < _offsets.Length) ? _offsets[_currentIndex] : Vector3.zero; 
                _uiMask.position = screenPosition + offset;
                               
                Vector2 newSize = (_currentIndex >= 0 && _currentIndex < _maskSize.Length) ? _maskSize[_currentIndex] : new Vector2( ); 
                _uiMask.sizeDelta = newSize;

                if (!_uiMask.gameObject.activeSelf)
                {
                    _uiMask.gameObject.SetActive(true); 
                }
            }
        }

        private void MarkerMoveToSceneObject(int index)
        {
            GameObject targetObject = _sceneObjectNames[index];

            if (targetObject != null)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetObject.transform.position);

                Vector3 newmarkeroffsets = (_currentIndex >= 0 && _currentIndex < _markeroffsets.Length) ? _markeroffsets[_currentIndex] : Vector3.zero; 
                _marker.position = screenPosition + newmarkeroffsets;
              
                if (!_marker.gameObject.activeSelf)
                {
                    _marker.gameObject.SetActive(true);
                }
            }
        }

        private void MoveToUIElement(int index)
        {
            RectTransform uiElement = _uiElementNames[index];

            if (uiElement != null)
            {
                RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector3 offset = (_currentIndex >= 0 && _currentIndex < _offsets.Length) ? _offsets[_currentIndex] : Vector3.zero;
                    _uiMask.position = rectTransform.position + offset;

                    Vector2 newSize = (_currentIndex >= 0 && _currentIndex < _maskSize.Length) ? _maskSize[_currentIndex] : new Vector2();
                    _uiMask.sizeDelta = newSize;                    
                    if (!_uiMask.gameObject.activeSelf)
                    {
                        _uiMask.gameObject.SetActive(true);
                    }
                }
            }               
        }

        private void MarkerMoveToUIElement(int index)
        {
            RectTransform uiElement = _uiElementNames[index];

            if (uiElement != null)
            {
                RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector3 newmarkeroffsets = (_currentIndex >= 0 && _currentIndex < _markeroffsets.Length) ? _markeroffsets[_currentIndex] : Vector3.zero; 
                    _marker.position = rectTransform.position + newmarkeroffsets;
                    if (!_marker.gameObject.activeSelf)
                    {
                        _marker.gameObject.SetActive(true);
                    }
                }
            }
        }

        private void ShowSceneObjcetDescriptionName(int index)
        {
            _descriptionNameText.text = _sceneObjectDescriptionName[index];
            if (!_descriptionNameText.gameObject.activeSelf)
            {
                _descriptionNameText.gameObject.SetActive(true);
            }
        }

        private void ShowSceneObjectDescription (int index)
        {
            _descriptionText.text = _sceneObjectDescription[index];
            if(!_descriptionText.gameObject.activeSelf)
            {
                _descriptionText.gameObject.SetActive(true);
            }
        }

        private void ShowUIDescriptionName(int index)
        {
            _descriptionNameText.text = _uiDescriptionName[index];
            if (!_descriptionNameText.gameObject.activeSelf)
            {
                _descriptionNameText.gameObject.SetActive(true);
            }
        }

        private void ShowUIDescription(int index)
        {
            _descriptionText.text = _uiDescription[index];
            if (!_descriptionText.gameObject.activeSelf)
            {
                _descriptionText.gameObject.SetActive(true);
            }
        }

        private void ClosePanel()
        {
            _panel.SetActive(false);
            OnTutorialFinished?.Invoke();

          _pauseHandler.SetPaused(false);
        }
    }
}



