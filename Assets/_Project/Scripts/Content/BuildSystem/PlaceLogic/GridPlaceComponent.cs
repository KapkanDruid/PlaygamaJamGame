using Cysharp.Threading.Tasks;
using NavMeshPlus.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class GridPlaceComponent
    {
        private Transform _pivotTransform;
        private NavMeshSurface _navMeshSurface;
        private GridPatternData _gridPatternData;
        private GridPlaceSystem _gridPlaceSystem;

        private List<Vector2Int> _gridPattern;

        private IPlaceComponentData _data;
        private IEntity _currentEntity;

        private bool _isSelected;

        public event Action OnPlaced;

        public IEnumerable<Vector2Int> GridPattern => _gridPattern;
        public Transform PivotTransform => _pivotTransform;

        public GridPlaceComponent(GridPlaceSystem gridPlaceSystem,
                                  IPlaceComponentData placeComponentData,
                                  NavMeshSurface navMeshSurface,
                                  IEntity currentEntity)
        {
            _gridPlaceSystem = gridPlaceSystem;
            _navMeshSurface = navMeshSurface;
            _currentEntity = currentEntity;
            _data = placeComponentData;

            _gridPatternData = _data.GridPattern;
            _pivotTransform = _data.PivotTransform;

            _gridPattern = _gridPatternData.GridPattern;
            _gridPattern.Add(Vector2Int.zero);
        }

        public void Initialize()
        {
            for (int i = 0; i < _data.PhysicObjects.Length; i++)
            {
                GameObject currentObject = _data.PhysicObjects[i];
                currentObject.SetActive(false);
            }
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

            ActivatePhysicAsync().Forget();
            OnPlaced?.Invoke();
        }

        private async UniTask ActivatePhysicAsync()
        {
            for (int i = 0; i < _data.PhysicObjects.Length; i++)
            {
                GameObject currentObject = _data.PhysicObjects[i];
                currentObject.SetActive(true);
            }

            try
            {
                await _navMeshSurface.BuildNavMeshAsync();
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        public async UniTask ReleaseAsync()
        {
            _currentEntity.ProvideComponent<MonoBehaviour>().gameObject.SetActive(false);

            try
            {
                await _navMeshSurface.BuildNavMeshAsync();
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}
