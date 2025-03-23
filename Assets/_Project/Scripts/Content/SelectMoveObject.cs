using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class SelectMoveObject : MonoBehaviour
    {
        private Camera _mainCamera;
        private bool _isDragging = false;
        private PauseHandler _pauseHandler;

        [Inject]
        public void Construct(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_pauseHandler.IsPaused)
                return;

            Dragging();

            MoveObject();
        }

        private void Dragging()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 clickPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (GetComponent<Collider2D>().OverlapPoint(clickPosition))
                {
                    _isDragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }
        }

        private void MoveObject()
        {
            if (!_isDragging)
                return;

            Vector2 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = newPosition;

        }
    }
}
