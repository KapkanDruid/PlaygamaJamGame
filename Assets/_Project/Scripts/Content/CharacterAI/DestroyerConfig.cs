using UnityEngine;

namespace Project.Content.CharacterAI
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "_Project/Destroyer")]
    public class DestroyerConfig : ScriptableObject, ICharacterConfig
    {
        [Header("Health System")]
        [SerializeField] private float _health;

        [Header("Movement")]
        [SerializeField] private float _speed;

        [Header("Attack System")]
        [Header("Melee")]
        [SerializeField] private int _damage;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private Vector2 _hitColliderSize;
        [SerializeField] private Vector2 _hitColliderOffset;

        [Header("Sensor System")]
        [SerializeField] private float _sensorRadius;

        public float Speed => _speed;
        public float Health => _health;
        public float AttackCooldown => _attackCooldown;
        public float SensorRadius => _sensorRadius;
        public int Damage => _damage;
        public Vector2 HitColliderSize => _hitColliderSize;
        public Vector2 HitColliderOffset => _hitColliderOffset;
    }
}