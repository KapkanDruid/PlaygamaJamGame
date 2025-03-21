using System;
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

        public bool TryGetTarget(out IEntity foundedEntity, out Transform targetTransform, Func<RaycastHit2D[], bool> additionalPredicate = null)
        {
            Vector2 origin = _data.SensorOrigin.position;
            Vector2 direction = Vector2.down;
            float radius = _data.SensorRadius;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, direction, 0);

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

                if (!flags.Contain(_data.TargetFlag))
                    continue;

                if (additionalPredicate != null)
                {
                    if (!additionalPredicate.Invoke(hits))
                        continue;
                }

                foundedEntity = entity;
                targetTransform = hits[i].transform;

                return true;
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