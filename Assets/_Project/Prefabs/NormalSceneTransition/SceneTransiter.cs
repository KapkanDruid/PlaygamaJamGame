using TMPro;
using UnityEngine;
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
        private AsyncOperation _loadingSceneOperation;

        public void SwitchToScene(string sceneName)
        {
            _componentAnimator.SetTrigger(AnimatorHashes.EndSceneTrigger);

            _loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            _loadingSceneOperation.allowSceneActivation = false;

            _loadingProgressBar.fillAmount = 0;
        }

        public void SwitchToScene(NameSceneConfig sceneConfig)
        {
            _componentAnimator.SetTrigger(AnimatorHashes.EndSceneTrigger);

            _loadingSceneOperation = SceneManager.LoadSceneAsync(sceneConfig.SceneName);

            _loadingSceneOperation.allowSceneActivation = false;

            _loadingProgressBar.fillAmount = 0;
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
            if (_loadingSceneOperation != null)
            {
                _loadingPercentage.text = Mathf.RoundToInt(_loadingSceneOperation.progress * 100) + "%";

                _loadingProgressBar.fillAmount = Mathf.Lerp(_loadingProgressBar.fillAmount, _loadingSceneOperation.progress,
                    Time.deltaTime * 5);
            }
        }

        public void OnAnimationOver()
        {
            _shouldPlayOpeningAnimation = true;

            _loadingSceneOperation.allowSceneActivation = true;
        }
    }
}
