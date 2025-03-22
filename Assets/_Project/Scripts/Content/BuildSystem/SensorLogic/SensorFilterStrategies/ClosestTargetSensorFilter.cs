using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class ClosestTargetSensorFilter : ISensorFilter
    {
        private Transform _origin;

        public ClosestTargetSensorFilter(Transform origin)
        {
            _origin = origin;
        }

        public bool TryToFilter(RaycastHit2D[] hits, out IEntity foundedEntity, out Transform targetTransform)
        {
            float minDistance = float.MaxValue;
            Transform closestObject = null;

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider.transform == null)
                    continue;

                float distance = Vector2.Distance(_origin.position, hit.collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestObject = hit.collider.transform;
                }
            }

            if (closestObject != null)
            {
                foundedEntity = closestObject.GetComponent<IEntity>();
                targetTransform = closestObject.transform;
                return true;
            }

            foundedEntity = null;
            targetTransform = null;
            return false;
        }
    }
}
