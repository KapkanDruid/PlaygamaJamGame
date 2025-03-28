using System;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class Alert
    {
        [SerializeField] private string _text;

        public string Text => _text;
    }
}
