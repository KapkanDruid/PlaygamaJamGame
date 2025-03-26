using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class SelectMoveObject : MonoBehaviour
    {
        private const string Ground = "Ground";

        private Camera _mainCamera;
        private bool _isDragging = false;
        private Vector3 _initialPosition;
        private Collider2D _currentCollider;
        private PauseHandler _pauseHandler;
        
        [Inject] 
        public void Construct(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;
        }

        private void Start()
        {
            _currentCollider = GetComponent<Collider2D>();
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
                    _initialPosition = transform.position;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                CheckCollisionAndReset();
            }
        }

        private void MoveObject()
        {
            if (!_isDragging)
                return;

            Vector2 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = newPosition;

        }

        private void CheckCollisionAndReset()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_currentCollider.bounds.center, _currentCollider.bounds.size, 0f);

            foreach (var collider in colliders)
            {
                if (collider != _currentCollider && !collider.CompareTag(Ground))
                {
                    transform.position = _initialPosition;
                    return;
                }
            }
        }
    }
}
