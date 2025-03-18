using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Content
{
    public class HealthBarView : MonoBehaviour, IHealthView
    {
        [SerializeField] private Image _fillBar;

        private Transform _target;
        private Canvas _canvas;

        [Inject]
        private void Construct(SceneData sceneData)
        {
            _canvas = sceneData.HealthBarCanvas;
        }

        public void Initialize()
        {
            var target = new GameObject();
            target.name = "HealthBarPoint";

            _target = target.transform;

            transform.parent = _canvas.transform;
        }

        private void Update()
        {
            if (_target == null)
                return;

            transform.position = _target.position;
        }

        public void SetHealth(float currentHealth, float maxHealth)
        {
            _fillBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
