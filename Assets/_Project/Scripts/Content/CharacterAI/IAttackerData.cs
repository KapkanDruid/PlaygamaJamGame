using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface IAttackerData
    {
        public Vector2 HitColliderOffset { get; }
        public float HitColliderSize { get; }
    }
}