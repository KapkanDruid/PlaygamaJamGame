using UnityEngine;

namespace Project.Content.CharacterAI
{
    public interface IPatrolling
    {
        public Transform FlagTransform { get; }

        public void SetFlag(Transform flag);
    }
}
