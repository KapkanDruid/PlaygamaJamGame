using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField] private Vector2Int _groundGridSize;

        private Canvas _healthBarCanvas;
        private SceneRecourses _recourses;

        public Canvas HealthBarCanvas => _healthBarCanvas;
        public Vector2Int GroundGridSize => _groundGridSize; 

        [Inject]
        private void Construct(SceneRecourses recourses)
        {
            _recourses = recourses;
        }

        public void Initialize()
        {
            _healthBarCanvas = GameObject.Instantiate(_recourses.Prefabs.HpBarCanvas);
        }
    }
}
