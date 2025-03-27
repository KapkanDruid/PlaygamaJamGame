using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class SelectMoveObject : MonoBehaviour
    {
        private const string Ground = "Ground";

        [SerializeField] private Collider2D _checkCollider;
        [SerializeField] private float _searchRadiusIncrement = 0.5f;
        [SerializeField] private float _maxSearchRadius = 10f;

        private Camera _mainCamera;
        private bool _isDragging = false;
        private Vector3 _initialPosition;
        private Collider2D _currentCollider; 
        private PauseHandler _pauseHandler;
        private SpriteRenderer _spriteRenderer;

        [Inject] 
        public void Construct(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;
        }

        private void Start()
        {
            _currentCollider = GetComponent<Collider2D>();
            _mainCamera = Camera.main;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            SetSpriteColor(Color.clear);
        }

        private void Update()
        {
            if (_pauseHandler.IsPaused)
                return;

            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 clickPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (_currentCollider.OverlapPoint(clickPosition))
                {
                    _isDragging = true;
                    _initialPosition = transform.position;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_isDragging)
                {
                    _isDragging = false;
                    FindNearestValidPosition();
                    SetSpriteColor(Color.clear);
                }
            }

            if (_isDragging)
            {
                MoveObject();
            }
        }

        private void MoveObject()
        {
            Vector2 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = newPosition;

            UpdateSpriteColor();
        }

        private void FindNearestValidPosition()
        {
            float searchRadius = _searchRadiusIncrement;
            Vector3 validPosition = _initialPosition;

            while (searchRadius <= _maxSearchRadius)
            {
                Vector2[] points = GeneratePointsAround(transform.position, searchRadius);

                for (int i = 0; i < points.Length; i++)
                {
                    if (IsValidPosition(points[i]))
                    {
                        validPosition = points[i];
                        transform.position = validPosition;
                        return;
                    }
                }

                searchRadius += _searchRadiusIncrement;
            }

            transform.position = validPosition;
        }

        private bool IsValidPosition(Vector2 position)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, _checkCollider.bounds.extents.magnitude);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != _checkCollider && !colliders[i].CompareTag(Ground))
                {
                    return false;
                }
            }

            return true;
        }

        private Vector2[] GeneratePointsAround(Vector2 center, float radius)
        {
            int pointCount = 8;
            Vector2[] points = new Vector2[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                float angle = i * Mathf.PI * 2 / pointCount;
                points[i] = new Vector2(center.x + Mathf.Cos(angle) * radius, center.y + Mathf.Sin(angle) * radius);
            }

            return points;
        }

        private void UpdateSpriteColor()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_checkCollider.bounds.center, _checkCollider.bounds.size, 0f);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != _checkCollider && !colliders[i].CompareTag(Ground))
                {
                    SetSpriteColor(Color.red);
                    return;
                }
            }

            SetSpriteColor(Color.green);
        }

        private void SetSpriteColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}
