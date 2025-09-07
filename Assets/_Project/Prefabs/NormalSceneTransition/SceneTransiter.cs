using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Content
{
    public class SceneTransiter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _loadingPercentage;
        [SerializeField] private Image _loadingProgressBar;

        private static bool _shouldPlayOpeningAnimation = false;

        private Animator _componentAnimator;
        private AsyncOperationHandle<SceneInstance>? _addressableSceneHandle;

        public void SwitchToScene(string sceneName)
        {
            _componentAnimator.SetTrigger(AnimatorHashes.EndSceneTrigger);

            _addressableSceneHandle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            _addressableSceneHandle.Value.Completed += OnSceneLoadComplete;

            _loadingProgressBar.fillAmount = 0;
        }

        public void SwitchToScene(NameSceneConfig sceneConfig)
        {
            SwitchToScene(sceneConfig.SceneName);
        }

        private void Start()
        {
            _componentAnimator = GetComponent<Animator>();

            if (_shouldPlayOpeningAnimation)
            {
                _componentAnimator.SetTrigger(AnimatorHashes.StartSceneTrigger);
                _loadingProgressBar.fillAmount = 1;

                _shouldPlayOpeningAnimation = false;
            }
        }

        private void Update()
        {
            if (_addressableSceneHandle.HasValue && !_addressableSceneHandle.Value.IsDone)
            {
                float progress = _addressableSceneHandle.Value.PercentComplete;

                _loadingPercentage.text = Mathf.RoundToInt(progress * 100) + "%";
                _loadingProgressBar.fillAmount = Mathf.Lerp(
                    _loadingProgressBar.fillAmount,
                    progress,
                    Time.deltaTime * 5
                );
            }
        }

        public void OnAnimationOver()
        {
            _shouldPlayOpeningAnimation = true;
        }

        private void OnSceneLoadComplete(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load the scene: {handle.OperationException}");
            }
        }
    }
}
