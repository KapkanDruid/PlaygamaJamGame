using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI
{
    public class CharacterHealthHandler : IDamageable
    {
        private ICharacterData _data;
        private Animator _animator;
        private EnemyDeadHandler _enemyDeadHandler;
        private float _health;
        private bool _isKnockedDown = false;
        private bool _isDead = false;

        public float Health => _health;

        [Inject]
        public void Construct(ICharacterData data,
                              Animator animator,
                              EnemyDeadHandler enemyDeadHandler)
        {
            _data = data;
            _animator = animator;
            _enemyDeadHandler = enemyDeadHandler;
        }

        public void Initialize()
        {
            _health = _data.Health;
        }

        public void TakeDamage(float damage, Action callback)
        {
            if (_isDead)
                return;

            if (_isKnockedDown)
                return;

            callback?.Invoke();

            _health -= damage;
            if ( _health <= 0)
            {
                _enemyDeadHandler.Death();
                _isDead = true;
                //_animator.SetBool(AnimatorHashes.IsDead, _isDead);
                return;
            }
        }

    }
}