using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class UpgradeEffectController
    {
        private GameObjectPooler _effectsPool;
        private GameObject _prefab;

        public UpgradeEffectController(SceneRecourses sceneRecourses)
        {
            _prefab = sceneRecourses.Prefabs.UpgradeObject;
        }

        public void Initialize()
        {
            _effectsPool = new GameObjectPooler(_prefab, 10, "UpgradeEffectObjects");
        }

        public void PlaySingleEffect(Vector2 position)
        {
            var effectObject = _effectsPool.Get();
            effectObject.transform.position = position;

            DeactivateObjectAsync(effectObject).Forget();
        }

        public void PlayTriangleEffect(Vector2 position, Vector2 size)
        {
            var points = GetTrianglePoints(position, size);

            foreach (var point in points)
            {
                var effectObject = _effectsPool.Get();
                effectObject.transform.position = point;

                DeactivateObjectAsync(effectObject).Forget();
            }
        }

        private async UniTask DeactivateObjectAsync(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out Animator animator))
            {
                Debug.Log("Animator null");
                gameObject.SetActive(false);
                return;
            }

            animator.SetTrigger(AnimatorHashes.ShowEffectTrigger);

            await animator.WaitForCurrentAnimationStateAsync(gameObject.GetCancellationTokenOnDestroy());

            gameObject.SetActive(false);
        }

        private Vector2[] GetTrianglePoints(Vector2 position, Vector2 size)
        {
            float radius = Mathf.Max(size.x, size.y) * 0.5f;
            Vector2[] points = new Vector2[3];

            for (int i = 0; i < 3; i++)
            {
                float angle = 2 * Mathf.PI / 3 * i - Mathf.PI / 2;
                points[i] = new Vector2(
                    position.x + radius * Mathf.Cos(angle),
                    position.y + radius * Mathf.Sin(angle)
                );
            }

            return points;
        }
    }
}
