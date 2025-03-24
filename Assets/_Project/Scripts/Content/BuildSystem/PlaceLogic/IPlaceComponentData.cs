using UnityEngine;

namespace Project.Content.BuildSystem
{
    public interface IPlaceComponentData
    {
        public Transform PivotTransform { get; }
        public SpriteRenderer[] SpriteRenderers { get; }
        public Transform[] ScalableObjects { get; }
        public GridPatternData GridPattern { get; }
        public GameObject[] PhysicObjects { get; }
    }
}
