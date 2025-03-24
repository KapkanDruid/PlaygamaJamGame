using UnityEngine;

namespace Project.Content.BuildSystem
{
    public interface ITurretShootData : IShooterData
    {
        public float RotateSpeed { get; }
        public float RotationThreshold { get; }
        public Transform RotationObject { get; }
    }
}