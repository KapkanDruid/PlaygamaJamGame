using System.Collections.Generic;
using UnityEngine;

namespace Project.Content
{
    public class FloatingTextHandler
    {
        private SceneRecourses _sceneRecourses;

        private Dictionary<FloatingTextConfig, MonoObjectPooler<FloatingText>> _textPoolers = new();

        public FloatingTextHandler(SceneRecourses sceneRecourses)
        {
            _sceneRecourses = sceneRecourses;
        }

        public void Initialize()
        {
            var textPrefabs = _sceneRecourses.Prefabs.FloatingTextPrefabs;

            var poolersParentObject = new GameObject("FloatingTextPools");

            foreach (var text in textPrefabs)
            {
                if (!_textPoolers.ContainsKey(text.Config))
                {
                    _textPoolers[text.Config] = new MonoObjectPooler<FloatingText>(10, poolersParentObject, new InstantiateObjectsSimple<FloatingText>(text));
                }
                else
                    Debug.LogError($"[FloatingTextHandler] Filed to create {text.name} pool! Found multiple configs with same type.");
            }
        }

        public void ShowText(FloatingTextConfig textConfig, Vector2 position, string massage)
        {
            var text = _textPoolers[textConfig].Get();

            text.Prepare(position, massage);
        }
    }
}
