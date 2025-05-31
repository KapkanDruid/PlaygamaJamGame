using System;
using UnityEngine;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    [Serializable]
    public class MainTargetAttackerConfigDto
    {
        public float Health;
        public float ExperiencePoints;
        public float Speed;
        public float DistanceToTarget;
        public int Damage;
        public float AttackCooldown;
        public float HitColliderSize;
        public Vector2 HitColliderOffset;
        public float SensorRadius;
        public MainTargetAttackerType Type;
    }
}