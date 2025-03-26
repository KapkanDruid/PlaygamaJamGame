using System.Collections.Generic;
using UnityEngine;


namespace Project.Content.UI
{
    public class ModifierIconsController : MonoBehaviour
    {
        private RectTransform[] _modifierIcons;
        private Vector2[] _positions;

        private HashSet<RectTransform> _placedIcons = new();

        private int _currentPositionIndex;

        private void Start()
        {
            var modifierIcons = GetComponentsInChildren<ModifierIcon>();
            List<Vector2> positions = new();
            List<RectTransform> transforms = new();

            foreach (var icon in modifierIcons) 
            {
                positions.Add(icon.GetComponent<RectTransform>().anchoredPosition);
                transforms.Add(icon.GetComponent<RectTransform>());
            }

            transforms.Reverse();
            positions.Reverse();

            _modifierIcons = transforms.ToArray();
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
