using System;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class BuildingHealthComponent : IDamageable, IDisposable
    {
        private IHealthData _data;
        private IHealthView _view;

        private float _currentHealth;
        private float _maxHealth;
        private bool _isDead = false;

        public event Action OnDead;
        public event Action OnTakeDamage;
        public float Health => _currentHealth;

        public BuildingHealthComponent(IHealthData data, IHealthView view)
        {
            _data = data;
            _view = view;
        }

        public void Initialize()
        {
            _maxHealth = _data.Health.Value;
            _currentHealth = _maxHealth;
            _view.SetHealth(_currentHealth, _maxHealth);

            _data.Health.OnValueChanged += OnMaxHealthChanged;
        }

        private void OnMaxHealthChanged(float changedMaxHealth)
        {
            float percent = (_currentHealth / _maxHealth) * 100;

            _maxHealth = changedMaxHealth;
            _currentHealth = (percent / 100f) * _maxHealth;

            _view.SetHealth(_currentHealth, _maxHealth);
        }

        public void TakeDamage(float damage, Action callback)
        {
            if (_isDead)
                return;
            OnTakeDamage?.Invoke();
            callback?.Invoke();

            UpdateHealth(-damage);
        }

        private void UpdateHealth(float healthModifier)
        {
            _currentHealth += healthModifier;

            _view.SetHealth(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                _isDead = true;
                OnDead?.Invoke();
                return;
            }
        }

        public void Dispose()
        {
            _data.Health.OnValueChanged -= OnMaxHealthChanged;
        }
    }
}
