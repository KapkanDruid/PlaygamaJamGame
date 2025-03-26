using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI
{
    public class DefensiveFlag : MonoBehaviour
    {
        [SerializeField] private int _capacity = 20;
        [SerializeField] private int _fullness = 0;
        [SerializeField] private float _coverage = 4f;

        public class Factory : PlaceholderFactory<DefensiveFlag> { }

        public int Capacity => _capacity;
        public int Fullness => _fullness;
        public float Coverage => _coverage;
        public bool IsFull => _fullness == _capacity;
        
        public void AddUnit()
        {
            if (_fullness < _capacity)
            {
                _fullness++;
            }
        }

        public void RemoveUnit()
        {
            if (_fullness > 0)
            {
                _fullness--;
            }
        }
    }
}