using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content
{
    public class Tutorial : MonoBehaviour
    {
        public static event Action OnTutorialFinished;

        public Sprite[] images;
        public Image targetImage;
        public GameObject panel;
        public Image radialSlider;

        private PauseHandler _pauseHandler;

        public float fillSpeed = 1f;

        private int currentIndex = 0;
        private bool isFilling = false;

        [Inject]
        private void Construct(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;
        }

        private void Start()
        {
            _pauseHandler.SetPaused(true);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && panel.activeSelf && !isFilling)
            {
                currentIndex++;

                if (currentIndex < images.Length)
                {
                    targetImage.sprite = images[currentIndex];
                }
                else
                {
                    ClosePanel();
                }
            }

            if (Input.GetKey(KeyCode.Space) && panel.activeSelf)
            {
                isFilling = true;
                radialSlider.fillAmount += fillSpeed * Time.deltaTime;

                if (radialSlider.fillAmount >= 1f)
                {
                    ClosePanel();
                    isFilling = false;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                isFilling = false;
                radialSlider.fillAmount = 0f;
            }
        }

        private void ClosePanel()
        {
            panel.SetActive(false);
            OnTutorialFinished?.Invoke();

            _pauseHandler.SetPaused(false);
        }
    }
}