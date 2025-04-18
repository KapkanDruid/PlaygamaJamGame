﻿using UnityEngine;

namespace Project.Content.CharacterAI.Infantryman
{
    [CreateAssetMenu(fileName = "AllyUnitConfig", menuName = "_Project/Config/Infantryman")]
    public class InfantrymanConfig : ScriptableObject, IAllyEntityConfig
    {
        [Header("Health System")]
        [SerializeField] private float _health;

        [Header("Movement")]
        [SerializeField] private float _speed;

        [Header("Attack System")]
        [Header("Range")]
        [SerializeField] private int _damage;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _attackRange;

        [Header("Entity Type")]
        [SerializeField] private AllyEntityType _type;

        [Header("Sensor System")]
        [SerializeField] private float _sensorRadius;
        [Header("Upgrade")]
        [SerializeField] private Sprite _upgradeSprite;


        public int Damage => _damage;
        public float Speed => _speed;
        public float Health => _health;
        public float SensorRadius => _sensorRadius;
        public float AttackRange => _attackRange;
        public float AttackCooldown => _attackCooldown;
        public AllyEntityType Type => _type;
        public Sprite UpgradeSprite => _upgradeSprite;
    }
}

