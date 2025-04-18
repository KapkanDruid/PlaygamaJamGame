﻿using System;
using UnityEditor;
using UnityEngine;

namespace Project.Content.CharacterAI
{
    public class CharacterHealthHandler : IDamageable
    {
        private Animator _animator;
        private EnemyDeadHandler _enemyDeadHandler;
        private float _health;
        private bool _isDead = false;

        public event Action<float> OnDamage;
        public float Health => _health;

        public CharacterHealthHandler(float health,
                              Animator animator,
                              EnemyDeadHandler enemyDeadHandler)
        {
            _health = health;
            _animator = animator;
            _enemyDeadHandler = enemyDeadHandler;
        }

        public void Reset()
        {
            _isDead = false;
            _animator.SetBool(AnimatorHashes.IsDead, _isDead);
        }

        public void TakeDamage(float damage, Action callback)
        {
            if (_isDead)
                return;

            callback?.Invoke();
            OnDamage?.Invoke(damage);

            _health -= damage;
            if ( _health <= 0)
            {
                _enemyDeadHandler.Death();
                _isDead = true;
                return;
            }
        }

    }
}