using UnityEngine;
using Zenject;

namespace Project.Content
{
    public class SceneData : MonoBehaviour
    {
        [SerializeField] private Vector2Int _groundGridSize;

        private SceneRecourses _recourses;

        public Vector2Int GroundGridSize => _groundGridSize; 

        [Inject]
        private void Construct(SceneRecourses recourses)
        {
            _recourses = recourses;
        }
    }
}
