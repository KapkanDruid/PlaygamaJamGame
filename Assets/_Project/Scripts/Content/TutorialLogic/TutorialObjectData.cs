using UnityEngine;
using System;
using UnityEngine.Localization;

namespace Project.Content
{
    [Serializable]
    public class TutorialObjectData : IDisposable
    {
        [SerializeField] private Transform _objectPosition;
        [SerializeField] private LocalizedString _localizedName;
        [SerializeField] private LocalizedString _localizedDescription;
        [SerializeField] private Vector2 _maskScale;
        [SerializeField] private Vector2 _maskPositionOffset;
        [SerializeField] private Vector2 _markerPositionOffset;

        private string _objectName;
        private string _objectDescription;

        public Transform ObjectPosition => _objectPosition;
        public string ObjectName => _objectName;
        public string ObjectDescription => _objectDescription;
        public Vector2 MaskScale => _maskScale;
        public Vector2 MaskPositionOffset => _maskPositionOffset;

        public Vector2 MarkerPositionOffset => _markerPositionOffset;

        public void Initialize()
        {
            _localizedName.StringChanged += OnLocalizedNameChanged;
            _localizedDescription.StringChanged += OnLocalizedDescriptionChanged;
        }

        private void OnLocalizedNameChanged(string value)
        {
            _objectName = value;
        }

        private void OnLocalizedDescriptionChanged(string value)
        {
            _objectDescription = value;
        }

        public void Dispose()
        {
            if (_localizedName != null)
                _localizedName.StringChanged -= OnLocalizedNameChanged;

            if (_localizedDescription != null)
                _localizedDescription.StringChanged -= OnLocalizedDescriptionChanged;
        }
    }
}



