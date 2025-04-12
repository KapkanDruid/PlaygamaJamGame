using System;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public class Alert
    {
        [SerializeField] private string _key;

        public string Key => _key;
    }
}
