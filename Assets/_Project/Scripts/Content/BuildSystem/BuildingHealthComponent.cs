using System;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class BuildingHealthComponent : IDamageable
    {
        private IHealthData _data;
        private IHealthView _view;

        private float _currentHealth;
        private bool _isDead = false;

        public event Action OnDead;
        public float Health => _currentHealth;

        public BuildingHealthComponent(IHealthData data, IHealthView view)
        {
            _data = data;
            _view = view;
        }

        public void Initialize()
        {
            _currentHealth = _data.Health;
            _view.SetHealth(_currentHealth, _data.Health);
        }

        public void TakeDamage(float damage, Action callback)
        {
            if (_isDead)
                return;

            callback?.Invoke();

            Debug.Log($"Building take {damage} damage, current health: {_currentHealth}");

            UpdateHealth(-damage);
        }

        private void UpdateHealth(float healthModifier)
        {
            _currentHealth += healthModifier;

            _view.SetHealth(_currentHealth, _data.Health);

            if (_currentHealth <= 0)
            {
                _isDead = true;
                OnDead?.Invoke();
                return;
            }
        }
    }
}
