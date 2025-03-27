using System;
using UnityEngine;

namespace Project.Content.CharacterAI
{
    public class CharacterSensor
    {
        private readonly ISensorData _data;
        private Transform _targetTransformToChase;
        private Transform _targetTransformToAttack;
        private IEntity _targetToChase;
        private IEntity _targetToAttack;
        private IEntity _currentTargetToChase;
        private IEntity _currentTargetToAttack;
        public IEntity TargetToChase => _targetToChase;
        public IEntity TargetToAttack => _targetToAttack;
        public Transform TargetTransformToChase => _targetTransformToChase;
        public Transform TargetTransformToAttack => _targetTransformToAttack;

        public event Action TargetDetected;
        public event Action HasTargetToAttack;

        public CharacterSensor(ISensorData data)
        {
            _data = data;
        }

        public void ResetTargets()
        {
            _targetToChase = null;
            _targetToAttack = null;
            _targetTransformToChase = null;
            _targetTransformToAttack = null;
        }

        public void ScanAreaToAttack()
        {
            ScanAndSetTarget(ref _targetToAttack,
                             ref _targetTransformToAttack,
                             _data.HitColliderSize,
                             _data.HitColliderOffset,
                             ref _currentTargetToAttack,
                             OnHasTargetToAttack);
        }

        public void TargetSearch()
        {
            ScanAndSetTarget(ref _targetToChase,
                             ref _targetTransformToChase,
                             _data.SensorRadius,
                             Vector2.zero,
                             ref _currentTargetToChase,
                             OnTargetDetected);
        }

        private void ScanAndSetTarget(ref IEntity target, ref Transform targetTransform, float size, Vector2 offset, ref IEntity currentTarget, Action onTargetFound)
        {
            (target, targetTransform) = Scan(_data.CharacterTransform.position, size, offset, true);

            if (target != null && targetTransform != null && target != currentTarget)
            {
                currentTarget = target;
                onTargetFound?.Invoke();
            }
        }

        private void OnTargetDetected()
        {
            TargetDetected?.Invoke();
        }

        private void OnHasTargetToAttack()
        {
            HasTargetToAttack?.Invoke();
        }

        private (IEntity target, Transform targetTransform) Scan(Vector2 position, float size, Vector2 offset, bool closest = false)
        {
            if (_data == null)
                return (null, null);

            Vector2 origin = position + offset;
            Vector2 direction = Vector2.right;
            float radius = size;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, direction);

            IEntity closestTarget = null;
            Transform closestTransform = null;
            float minDistance = float.MaxValue;

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

                foreach (var flag in _data.EnemyFlag)
                {
                    if (flags.Contain(flag))
                    {
                        isEnemy = true;
                        break;
                    }
                }

                if (!isEnemy)
                    continue;

                if (!closest)
                    return (entity, hits[i].transform);

                float distance = Vector2.Distance(position, hits[i].transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = entity;
                    closestTransform = hits[i].transform;
                }
            }

            if (closest)
                return (closestTarget, closestTransform);

            return (null, null);
        }
    }
}
