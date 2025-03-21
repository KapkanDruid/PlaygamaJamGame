using UnityEngine;

namespace Project.Content.BuildSystem
{
    public interface ITurretShootData
    {
        public float FireRate { get; }
        public float RotateSpeed { get; }
        public float RotationThreshold { get; }
        public Transform RotationObject { get; }
        public Transform ShootPoint { get; }
        public IDirectProjectileData ProjectileData { get; }
        public ISensorData SensorData { get; }
    }
}