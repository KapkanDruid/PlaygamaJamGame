using UnityEngine;

namespace Project.Content.BuildSystem
{
    public interface ISensorData
    {
        public Transform SensorOrigin { get; }
        public float SensorRadius { get; }
        public IEntity ThisEntity { get; }
        public EntityFlags TargetFlag { get; }
    }
}