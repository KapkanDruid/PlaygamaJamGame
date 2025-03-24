using Project.Content.UI;
using System;
using UnityEngine;

namespace Project.Content
{
    public class LevelExperienceController : IDisposable
    {
        private LevelExperienceConfig _config;
        private LevelExperienceView _view;
        private CardsPopupView _cardsPopupView;
        private SceneRecourses _recourses;

        private GameObjectPooler _experienceObjectPool;

        private float _currentCoefficient;
        private int _currentLevel;
        private float _currentPoints;
        private float _pointsToNextLevel;

        private float CurrentCoefficient
        {
            get => _currentCoefficient;
            set
            {
                if (value <= 0)
                    return;

                _currentCoefficient = value;
            }
        }

        public LevelExperienceController(SceneRecourses recourses, CardsPopupView cardsPopupView, LevelExperienceView view)
        {
            _recourses = recourses;
            _config = _recourses.Configs.LevelExperienceConfig;
            _cardsPopupView = cardsPopupView;
            _view = view;

            var experienceObjectPrefab = _recourses.Prefabs.ExperienceObject;
            _experienceObjectPool = new GameObjectPooler(experienceObjectPrefab, 20, "ExperienceObjects");

            _cardsPopupView.OnPopupClosed += OnCardsClosed;
        }


        public void Initialize()
        {
            _currentLevel = 1;

            _view.SetExperienceBar(_currentPoints, _pointsToNextLevel, _currentLevel);

            foreach (var levelData in _config.LevelExperienceData)
            {
                if (_currentLevel == levelData.Level)
                {
                    _pointsToNextLevel = levelData.PointsToReach;
                    return;
                }
            }

            Debug.LogError("[LevelExperienceHandler] No valid experience data found");
        }

        public void OnEnemyDied(Vector2 position, float experiencePoints)
        {
            var viewObject = _experienceObjectPool.Get().transform;
            viewObject.position = position;
            _view.ShowColletObject(viewObject, () =>
            {
                _currentPoints += experiencePoints;

                if (_currentPoints >= _pointsToNextLevel)
                    LevelReached();
                else
                    _view.SetExperienceBar(_currentPoints, _pointsToNextLevel, _currentLevel);
            });
        }

        private void LevelReached()
        {
            _currentPoints = 0;
            _currentLevel++;

            _view.SetExperienceBar(_pointsToNextLevel, _pointsToNextLevel, _currentLevel);

            UpdateLocalData();
            _cardsPopupView.Show();
        }

        private void OnCardsClosed()
        {
            _view.SetExperienceBar(_currentPoints, _pointsToNextLevel, _currentLevel);
        }

        private void UpdateLocalData()
        {
            foreach (var levelData in _config.LevelExperienceData)
            {
                if (_currentLevel == levelData.Level)
                {
                    _pointsToNextLevel = levelData.PointsToReach;
                    return;
                }
            }

            var configListMsxIndex = _config.LevelExperienceData.Count - 1;

            _pointsToNextLevel = _config.LevelExperienceData[configListMsxIndex].PointsToReach;

            Debug.Log("[LevelExperienceHandler] No more experience data. Using last available data");
        }

        public void IncreaseCoefficient(float coefficient) => CurrentCoefficient += coefficient;
        public void DecreaseCoefficient(float coefficient) => CurrentCoefficient -= coefficient;

        public void Dispose()
        {
            _cardsPopupView.OnPopupClosed -= OnCardsClosed;
        }
    }
}
