using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Zenject;
using System;
using System.Reflection;
using TMPro;
using static UnityEngine.Rendering.VolumeComponent;
using UnityEngine.Localization.Components;

namespace Project.Content
{
    public class TutorialSwithObjectController : MonoBehaviour
    {
        public static event Action OnTutorialFinished;

       [SerializeField] private Transform[] _tutorialTransform;
       [SerializeField] private string[] _tutorialTransformName;
       [SerializeField] private string[] _tutorialTransformDescription;
        
        [SerializeField] private TextMeshProUGUI _nameDescriptionText;        
        [SerializeField] private TextMeshProUGUI _descriptionText;
        
        [SerializeField] private RectTransform _uiMask;
        [SerializeField] private RectTransform _marker;                        

        [SerializeField] private GameObject _tutorialPanel;

        [SerializeField] private Image _radialSlider;
        [SerializeField] private float _fillSpeed = 1f;
        private bool isFilling = false;

        private Vector2[] _maskSizeTransform = new Vector2[] {
           new Vector2(220f, 170f),
           new Vector2(225f,170f),
           new Vector2(125f,125f),
           new Vector2(100f,225f),
           new Vector2(1150f, 170f),
           new Vector2(200f,200f),
           new Vector2(100f,100f),
           new Vector2(100f,100f)
       };

        private Vector3[] _maskTransformOffset = new Vector3[] {
        new Vector3(10f,20f,0),
        new Vector3(10f, 0f, 0f),
        new Vector3(20f,20f,0f),
        new Vector3(20f, 80f,0f),
        new Vector3(10f,0f,0f),
        new Vector3(20f,0f,0f),
        new Vector3(0f,0f,0f),
        new Vector3(-20f,-40f,0f)
        };

        private Vector3 _markerOffset = new Vector3(-50f, -150f, 0f);

        private int _currentIndex = 0;

        private PauseHandler _pauseHandler;
        
        public Camera _mainCamera;       
        
        [Inject]
        private void Construct(PauseHandler pauseHandler/*,  Camera _mainCamera*/)
        {
            _pauseHandler = pauseHandler;
            //_mainCamera = Camera.main;
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
            if (_nameDescriptionText != null) {
                  _nameDescriptionText.gameObject.SetActive(false);
             }            
            if (_descriptionText != null) { 
            _descriptionText.gameObject.SetActive(false);
            }           

            _pauseHandler.SetPaused(true);
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
            if (_currentIndex < _tutorialTransform.Length)
            {
                MoveMaskToTransform(_currentIndex);
                MoveMarkerToTransform(_currentIndex);
                ShowTutorialDescriptionName(_currentIndex);
                ShowTutorialDescription(_currentIndex);
                _currentIndex++;
            }
            else
            {
                ClosePanel();
            }
        }
        private void MoveMaskToTransform(int index)
        {
            Transform targetObject = _tutorialTransform[index];

            if (targetObject is RectTransform rectTransform)
            {
                Vector3 offset = (_currentIndex >= 0 && _currentIndex < _maskTransformOffset.Length) ? _maskTransformOffset[_currentIndex] : Vector3.zero;

                _uiMask.position = rectTransform.position;

                Vector2 newSize = (_currentIndex >= 0 && _currentIndex < _maskSizeTransform.Length) ? _maskSizeTransform[_currentIndex] : new Vector2();

                _uiMask.sizeDelta = newSize;

                if (!_uiMask.gameObject.activeInHierarchy)
                {
                    _uiMask.gameObject.SetActive(true);
                }
            }
            else
            {
                Vector3 screenPosition = _mainCamera.WorldToScreenPoint(targetObject.transform.position);

                Vector3 offset = (_currentIndex >= 0 && _currentIndex < _maskTransformOffset.Length) ? _maskTransformOffset[_currentIndex] : Vector3.zero;
                _uiMask.position = screenPosition + offset;

                Vector2 newSize = (_currentIndex >= 0 && _currentIndex < _maskSizeTransform.Length) ? _maskSizeTransform[_currentIndex] : new Vector2();
                _uiMask.sizeDelta = newSize;

                if (!_uiMask.gameObject.activeInHierarchy)
                {
                    _uiMask.gameObject.SetActive(true);
                }
            }
        }

        private void MoveMarkerToTransform(int index)
        {
            Transform targetObject = _tutorialTransform[index];

            if (targetObject is RectTransform rectTransform)
            {

                Vector3 offset = (_currentIndex >= 0 && _currentIndex < _maskTransformOffset.Length) ? _maskTransformOffset[_currentIndex] : Vector3.zero;

                _marker.position = rectTransform.position + _markerOffset;                

                if (!_marker.gameObject.activeInHierarchy)
                {
                    _marker.gameObject.SetActive(true);
                }
            }
            else
            {
                Vector3 screenPosition = _mainCamera.WorldToScreenPoint(targetObject.transform.position);
                                
                _marker.position = screenPosition + _markerOffset;              

                if (!_marker.gameObject.activeInHierarchy)
                {
                    _marker.gameObject.SetActive(true);
                }
            }            
        }           

        private void ShowTutorialDescriptionName(int index)
        {
            _nameDescriptionText.text = _tutorialTransformName[index];
            
            if (!_nameDescriptionText.gameObject.activeInHierarchy)
            {
                _nameDescriptionText.gameObject.SetActive(true);
            }
        }

        private void ShowTutorialDescription (int index)
        {
            _descriptionText.text = _tutorialTransformDescription[index];
            if(!_descriptionText.gameObject.activeInHierarchy)
            {
                _descriptionText.gameObject.SetActive(true);
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



