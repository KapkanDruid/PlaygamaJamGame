using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.UI
{
    public class ModifierIconsController : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _modifierIcons;
        private Vector2[] _positions;

        private HashSet<RectTransform> _placedIcons = new();

        private int _currentPositionIndex;

        private void Start()
        {
            List<Vector2> positions = new();

            foreach (var icon in _modifierIcons) 
            {
                positions.Add(icon.anchoredPosition);
            }

            _positions = positions.ToArray();
        }

       private void Update()
        {
            foreach (var icon in _modifierIcons)
            {
                if (icon.gameObject.activeSelf && !_placedIcons.Contains(icon))
                {
                    PlaceObject(icon);
                }
            }
        }

        private void PlaceObject(RectTransform icon)
        {
            if (_currentPositionIndex < _positions.Length)
            {
                icon.anchoredPosition = _positions[_currentPositionIndex];
                _placedIcons.Add(icon);
                _currentPositionIndex++;
            }
        }
    }
}
