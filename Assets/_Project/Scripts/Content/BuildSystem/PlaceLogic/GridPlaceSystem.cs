using Project.Architecture;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class GridPlaceSystem : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;

        private HashSet<Vector2Int> _occupiedCells = new();

        private Grid _grid;
        private Camera _mainCamera;
        private GridPlaceComponent _placeComponent;
        private InputSystemActions _inputSystemActions;

        public event Action OnPlaceComponentPlaced;

        private Vector2 _pointerPosition;

        private bool _isAnySelected;
        private bool _inputSelected;

        [Inject]
        private void Construct(Grid grid, Camera mainCamera, InputSystemActions inputSystemActions)
        {
            _grid = grid;
            _mainCamera = mainCamera;
            _inputSystemActions = inputSystemActions;

            _inputSystemActions.Player.Pointer.performed += ReadInputPointer;
            _inputSystemActions.Player.Select.performed += context => _inputSelected = true;
            _inputSystemActions.Player.Select.canceled += context => _inputSelected = false;
        }

        public void StartPlacing(GridPlaceComponent placeComponent)
        {
            _placeComponent = placeComponent;
            _placeComponent.Select();
            _isAnySelected = true;
        }

        public void RemoveFromGrid(GridPlaceComponent placeComponent)
        {
            var placeComponentGridPattern = placeComponent.GridPattern;
            Vector2Int componentMainCellPosition = (Vector2Int)_grid.WorldToCell(placeComponent.PivotTransform.position);

            foreach (var patternCell in placeComponentGridPattern)
            {
                _occupiedCells.Remove(componentMainCellPosition + patternCell);
            }
        }

        public void PLaceOnGrid(GridPlaceComponent placeComponent)
        {
            OccupyCells(placeComponent);
        }

        private void Update()
        {
            if (!_isAnySelected)
                return;

            var cellPosition = _grid.WorldToCell(_pointerPosition);

            var cellCenterPosition = _grid.GetCellCenterWorld(cellPosition);

            _placeComponent.MoveSelected(cellCenterPosition, CanBePlaced());

            if (!CanBePlaced())
                return;

            if (!_inputSelected)
                return;

            PlaceSelected();
        }

        private void PlaceSelected()
        {
            OccupyCells(_placeComponent);

            _placeComponent = null;
            _isAnySelected = false;

            OnPlaceComponentPlaced?.Invoke();
        }

        private void OccupyCells(GridPlaceComponent placeComponent)
        {
            placeComponent.Place();

            var placeComponentGridPattern = placeComponent.GridPattern;
            Vector2Int componentMainCellPosition = (Vector2Int)_grid.WorldToCell(placeComponent.PivotTransform.position);

            foreach (var patternCell in placeComponentGridPattern)
            {
                _occupiedCells.Add(componentMainCellPosition + patternCell);
            }
        }

        private bool CanBePlaced()
        {
            if (!_isAnySelected)
                return false;

            var placeComponentGridPattern = _placeComponent.GridPattern;
            Vector2Int componentMainCellPosition = (Vector2Int)_grid.WorldToCell(_placeComponent.PivotTransform.position);

            foreach (var patternCell in placeComponentGridPattern)
            {
                var checkPosition = componentMainCellPosition + patternCell;

                if (checkPosition.x > _gridSize.x / 2 || checkPosition.x < -(_gridSize.x / 2))
                    return false;

                if (checkPosition.y > _gridSize.y / 2 || checkPosition.y < -(_gridSize.y / 2))
                    return false;
            }

            foreach (var patternCell in placeComponentGridPattern)
            {
                if (_occupiedCells.Contains(componentMainCellPosition + patternCell))
                    return false;
            }

            return true;
        }

        private void ReadInputPointer(InputAction.CallbackContext context)
        {
            var interactionPosition = context.ReadValue<Vector2>();

            _pointerPosition = _mainCamera.ScreenToWorldPoint(interactionPosition);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_grid == null)
                return;

            if (CanBePlaced())
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;

            Gizmos.DrawWireCube(_grid.GetCellCenterWorld(_grid.WorldToCell(_pointerPosition)), Vector3.one);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_pointerPosition, 0.2f);

            for (float x = -(_gridSize.x / 2); x <= _gridSize.x/2; x++)
            {
                for (float y = -(_gridSize.y / 2); y <= _gridSize.y/2; y++)
                {
                    Vector3 pointPosition = _grid.GetCellCenterWorld(_grid.WorldToCell(new Vector3(x, y, 0)));

                    Gizmos.DrawSphere(pointPosition, 0.1f);
                }
            }

            Gizmos.color = Color.red;

            foreach (var cell in _occupiedCells)
            {
                Gizmos.DrawWireCube(_grid.GetCellCenterWorld(_grid.WorldToCell(new Vector3(cell.x, cell.y, 0))), Vector3.one);
            }
        }
#endif
        private void OnDestroy()
        {
            _inputSystemActions.Player.Pointer.performed -= ReadInputPointer;
        }
    }
}
