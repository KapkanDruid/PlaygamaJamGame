using Project.Content.BuildSystem;
using UnityEngine;

namespace Project.Content
{
    public interface IShooterData
    {
        public float ReloadTime { get; }
        public Transform ShootPoint { get; }
        public IDirectProjectileData ProjectileData { get; }
        public ISensorData SensorData { get; }
    }
}