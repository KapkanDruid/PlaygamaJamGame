using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class GridPlaceComponent
    {
        private Transform _pivotTransform;
        private GridPlaceSystem _gridPlaceSystem;
        private GridPatternData _gridPatternData;

        private List<Vector2Int> _gridPattern;

        private IPlaceComponentData _data;

        private bool _isSelected;

        public event Action OnPlaced;

        public IEnumerable<Vector2Int> GridPattern => _gridPattern;
        public Transform PivotTransform => _pivotTransform;

        public GridPlaceComponent(GridPlaceSystem gridPlaceSystem, IPlaceComponentData placeComponentData)
        {
            _gridPlaceSystem = gridPlaceSystem;
            _data = placeComponentData;

            _gridPatternData = _data.GridPattern;
            _pivotTransform = _data.PivotTransform;

            _gridPattern = _gridPatternData.GridPattern;
            _gridPattern.Add(Vector2Int.zero);
        }

        public void Select()
        {
            _isSelected = true;
        }

        public void MoveSelected(Vector3 position, bool canBePlaced)
        {
            _pivotTransform.position = position;

            if (canBePlaced)
            {
                for (int i = 0; i < _data.SpriteRenderers.Length; i++)
                {
                    _data.SpriteRenderers[i].color = Color.green;
                }
            }
            else
            {
                for (int i = 0; i < _data.SpriteRenderers.Length; i++)
                {
                    _data.SpriteRenderers[i].color = Color.red;
                }
            }
        }

        public void Place()
        {
            for (int i = 0; i < _data.SpriteRenderers.Length; i++)
            {
                _data.SpriteRenderers[i].color = Color.white;
            }

            //Активировать логику 
            OnPlaced?.Invoke();
        }

        public void Release()
        {
            //Обновить NavMesh
        }
    }
}
