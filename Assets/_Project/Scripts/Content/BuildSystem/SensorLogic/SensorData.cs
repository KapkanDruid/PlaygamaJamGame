using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class SensorData : ISensorData
    {
        private Transform _sensorOrigin;
        private float sensorRadius;
        private IEntity thisEntity;
        private EntityFlags[] targetFlag;

        public Transform SensorOrigin { get => _sensorOrigin; set => _sensorOrigin = value; }
        public float SensorRadius { get => sensorRadius; set => sensorRadius = value; }
        public IEntity ThisEntity { get => thisEntity; set => thisEntity = value; }
        public EntityFlags[] TargetFlag { get => targetFlag; set => targetFlag = value; }
    }
}