using System;

namespace Project.Content.BuildSystem
{
    [Serializable]
    public class TurretConfigDto
    {
        public float MaxHealth;
        public float FireRate;
        public float RotateSpeed;
        public float RotationThreshold;
        public float ProjectileSpeed;
        public float ProjectileDamage;
        public float ProjectileLifeTime;
        public float SensorRadius;
        public TurretType Type;
    }
}