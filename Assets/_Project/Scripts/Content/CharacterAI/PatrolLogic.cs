using UnityEngine;

namespace Project.Content.CharacterAI.Infantryman
{
    public class PatrolLogic
    {
        public Vector3 GetRandomPoint(Vector3 center, float radius)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float distance = Random.Range(0f, radius);
            Vector3 randomPoint = new Vector3(
                center.x + Mathf.Cos(angle) * distance,
                center.y,
                center.z + Mathf.Sin(angle) * distance
            );
            return randomPoint;
        }
    }
}

