using System;
using UnityEngine;

namespace Project.Content.CharacterAI.Destroyer
{
    [Serializable]
    public class DestroyerConfigDto
    {
        public int Damage;
        public float Health;
        public float ExperiencePoints;
        public float Speed;
        public float DistanceToTarget;
        public float AttackCooldown;
        public float SensorRadius;
        public float HitColliderSize;
        public Vector2 HitColliderOffset;
        public DestroyerType Type;
    }
}

