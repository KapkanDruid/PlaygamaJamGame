using Project.Content.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Content
{
    public class FloatingTextHandler
    {
        private readonly FloatingTextPoolFactory _poolFactory;

        private MonoObjectPooler<FloatingText> _textPool;

        private Dictionary<FloatingTextConfig, MonoObjectPooler<FloatingText>> _textPoolers = new();

        public FloatingTextHandler(FloatingTextPoolFactory poolFactory)
        {
            _poolFactory = poolFactory;
        }

        public void Initialize()
        {
            _textPool = _poolFactory.Create();
        }

        public void ShowText(FloatingTextConfig textConfig, Vector2 position, string massage)
        {
            var textObject = _textPool.GetByFilter(new FilterByPredicate<FloatingText>(item => textConfig == item.Config));

            textObject.Prepare(position, massage);
        }
    }
}
