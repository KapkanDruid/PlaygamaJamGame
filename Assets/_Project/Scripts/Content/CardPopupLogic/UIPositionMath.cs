using UnityEngine;

namespace Project.Content.UI
{
    public class UIPositionMath
    {
        private RectTransform _localRectTransform;
        private RectTransform _canvasRectTransform;
        private Vector2 _startPosition;
        public Vector2 StartPosition => _startPosition;

        public UIPositionMath(RectTransform localRectTransform, RectTransform canvasRectTransform)
        {
            _localRectTransform = localRectTransform;
            _canvasRectTransform = canvasRectTransform;
        }

        public Vector2 DetermineHidePosition(float hideOffset)
        {
            _startPosition = _localRectTransform.anchoredPosition;

            float canvasHeight = _canvasRectTransform.rect.height;
            float panelHeight = _localRectTransform.rect.height;

            float currentTop = _localRectTransform.anchoredPosition.y + (panelHeight / 2f);
            float canvasBottom = -canvasHeight / 2f;

            float delta = (currentTop - canvasBottom) + hideOffset;
            Vector2 hiddenPosition = new Vector2(_localRectTransform.anchoredPosition.x, _localRectTransform.anchoredPosition.y - delta);
            return hiddenPosition;
        }
    }
}
