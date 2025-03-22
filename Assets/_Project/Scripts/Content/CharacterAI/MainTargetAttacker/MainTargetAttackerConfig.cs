using UnityEngine;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "_Project/Config/MainTargetAttacker")]
    class MainTargetAttackerConfig : ScriptableObject, ICharacterConfig
    {
        [Header("Health System")]
        [SerializeField] private float _health;

        [Header("Movement")]
        [SerializeField] private float _speed;
        [SerializeField] private float _distanceToTarget;

        [Header("Attack System")]
        [Header("Melee")]
        [SerializeField] private int _damage;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _hitColliderSize;
        [SerializeField] private Vector2 _hitColliderOffset;

        [Header("Sensor System")]
        [SerializeField] private float _sensorRadius;

        public int Damage => _damage;
        public float Speed => _speed;
        public float Health => _health;
        public float SensorRadius => _sensorRadius;
        public float AttackCooldown => _attackCooldown;
        public float HitColliderSize => _hitColliderSize;
        public float DistanceToTarget => _distanceToTarget;
        public Vector2 HitColliderOffset => _hitColliderOffset;
    }
}
