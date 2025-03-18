using System;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class BuildingHealthComponent : IDamageable
    {
        private IHealthData _data;
        private float _health;
        private bool _isDead = false;

        public event Action OnDead;
        public float Health => _health;

        public BuildingHealthComponent(IHealthData data)
        {
            _data = data;
        }

        public void Initialize()
        {
            _health = _data.Health;
        }

        public void TakeDamage(float damage, Action callback)
        {
            if (_isDead)
                return;

            callback?.Invoke();

            Debug.Log($"Building take {damage} damage, current health: {_health}");

            _health -= damage;

            if (_health <= 0)
            {
                _isDead = true;
                OnDead?.Invoke();
                return;
            }
        }
    }
}
