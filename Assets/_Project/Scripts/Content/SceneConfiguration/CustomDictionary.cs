using System;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class CustomDictionary<TKey, TValue>
    {
        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;

        public TKey Key => _key;
        public TValue Value => _value;
    }
}