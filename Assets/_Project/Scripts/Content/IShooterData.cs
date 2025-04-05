using Project.Content.BuildSystem;
using Project.Content.ProjectileSystem;
using UnityEngine;

namespace Project.Content
{
    public interface IShooterData
    {
        public float ReloadTime { get; }
        public Transform ShootPoint { get; }
        public IProjectileData ProjectileData { get; }
        public ISensorData SensorData { get; }
    }
}