using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Project.Content
{
    public class SceneTrasitionHandler : MonoBehaviour
    {
        private const string MainMenu = "MainMenu";
        private const string GameLoop = "GameLoop";


        [Header("Restart Button")]
        [SerializeField] private Button _restart;
        [Header("Main Menu Button")]
        [SerializeField] private Button _mainMenu;
        [Header("GameLoop Button")]
        [SerializeField] private Button _gameLoop;
        [Header("Scene Configurations")]
        [SerializeField] private List<NameSceneConfig> _sceneConfigs;


        private void Start()
        {
            _restart?.onClick.AddListener(RestartScene);
            _mainMenu?.onClick.AddListener(MainMenuScene);
            _gameLoop?.onClick.AddListener(GameLoopScene);
        }

        private void RestartScene()
        {
            DG.Tweening.DOTween.KillAll();
            string currentSceneName = SceneManager.GetActiveScene().name;
            LoadSceneByName(currentSceneName);
        }

        private void MainMenuScene()
        {
            DG.Tweening.DOTween.KillAll();
            LoadSceneByName(MainMenu);
        }
        
        private void GameLoopScene()
        {
            DG.Tweening.DOTween.KillAll();
            LoadSceneByName(GameLoop);
        }

        private void LoadSceneByName(string sceneName)
        {
            NameSceneConfig config = _sceneConfigs.Find(config => config.SceneName == sceneName);
            if (config != null)
            {
                SceneManager.LoadScene(config.SceneName);
            }
            else
            {
                Debug.LogError($"Scene '{sceneName}' not found in the configuration list.");
            }
        }
    }
}
