using UnityEngine;

namespace Project.Content.BuildSystem
{
    [CreateAssetMenu(fileName = "TurretConfig", menuName = "_Project/Config/TurretConfig")]
    public class TurretConfig : ScriptableObject
    {
        [Header("Static values")]
        [SerializeField] private TurretType _type;  
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _rotationThreshold;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileLifeTime;

        [Header("Runtime-modified values")]
        [SerializeField] private float _projectileDamage;
        [SerializeField] private float _sensorRadius;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _fireRate;

        public float MaxHealth => _maxHealth;
        public float FireRate => _fireRate;
        public float RotateSpeed => _rotateSpeed;
        public float ProjectileLifeTime => _projectileLifeTime;
        public float ProjectileSpeed => _projectileSpeed;
        public float ProjectileDamage => _projectileDamage;
        public float SensorRadius => _sensorRadius;
        public float RotationThreshold => _rotationThreshold; 
        public TurretType Type => _type;
    }
}