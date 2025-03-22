using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI
{
    public class CharacterHealthHandler : IDamageable
    {
        private Animator _animator;
        private EnemyDeadHandler _enemyDeadHandler;
        private float _health;
        private bool _isDead = false;

        public float Health => _health;

        public CharacterHealthHandler(float health,
                              Animator animator,
                              EnemyDeadHandler enemyDeadHandler)
        {
            _health = health;
            _animator = animator;
            _enemyDeadHandler = enemyDeadHandler;
        }

        public void TakeDamage(float damage, Action callback)
        {
            if (_isDead)
                return;

            callback?.Invoke();

            _health -= damage;
            Debug.Log("Health: " + _health);
            if ( _health <= 0)
            {
                _enemyDeadHandler.Death();
                _isDead = true;
                _animator.SetBool(AnimatorHashes.IsDead, _isDead);
                return;
            }
        }

    }
}