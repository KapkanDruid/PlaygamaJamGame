using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.BuildSystem
{
    public class TargetSensor : IGizmosDrawerOnSelected
    {
        private ISensorData _data;
        private Color _gizmoColor;

        public TargetSensor(ISensorData sensorData, Color gizmoColor = default)
        {
            _data = sensorData;
            _gizmoColor = gizmoColor;
        }

        public bool TryGetTarget(out IEntity foundedEntity, out Transform targetTransform, ISensorFilter additionalFilter = null)
        {
            Vector2 origin = _data.SensorOrigin.position;
            Vector2 direction = Vector2.down;
            float radius = _data.SensorRadius;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, direction, 0);
            List<RaycastHit2D> hitsForFilter = new();

            int count = hits.Length;
            for (int i = 0; i < count; i++)
            {
                if (!hits[i].collider.TryGetComponent(out IEntity entity))
                    continue;

                if (entity == _data.ThisEntity)
                    continue;

                Flags flags = entity.ProvideComponent<Flags>();

                if (flags == null)
                    continue;

                bool isEnemy = false;

                if (flags.Contain(_data.TargetFlag))
                {
                    isEnemy = true;
                }

                if (!isEnemy)
                    continue;

                if (additionalFilter != null)
                {
                    hitsForFilter.Add(hits[i]);
                    continue;
                }

                foundedEntity = entity;
                targetTransform = hits[i].transform;

                return true;
            }

            if (additionalFilter != null && hitsForFilter.Count > 0)
            {
                if (additionalFilter.TryToFilter(hitsForFilter.ToArray(), out IEntity filteredEntity, out Transform filteredTransform))
                {
                    foundedEntity = filteredEntity;
                    targetTransform = filteredTransform;
                    return true;
                }
            }

            foundedEntity = null;
            targetTransform = null;
            return false;
        }

        public void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireSphere(_data.SensorOrigin.position, _data.SensorRadius);
#endif
        }
    }
}