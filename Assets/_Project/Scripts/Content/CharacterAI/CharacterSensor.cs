using System;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI
{
    public class CharacterSensor : ITickable
    {
        protected readonly ISensorData _data;
        protected Transform _targetTransformToChase;
        protected Transform _targetTransformToAttack;
        protected IEntity _targetToChase;
        protected IEntity _targetToAttack;
        protected IEntity _currentTargetToChase;
        protected IEntity _currentTargetToAttack;
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

        public virtual void Tick()
        {
            CheckFlagsOnSensor();
        }

        protected virtual void CheckFlagsOnSensor()
        {
            foreach (var flag in _data.Flags.Values)
            {
                switch (flag)
                {
                    case EntityFlags.Player:
                        break;
                    case EntityFlags.Enemy:
                        TargetSearch();
                        ScanAreaToAttack();
                        break;
                    case EntityFlags.FriendlyUnit:
                        TargetSearch();
                        ScanAreaToAttack();
                        break;
                    case EntityFlags.MainBuilding:
                        TargetSearch();
                        break;
                    case EntityFlags.Building:
                        TargetSearch();
                        break;
                    case EntityFlags.Another:
                        break;
                }
            }
        }

        protected void ScanAreaToAttack()
        {
            (_targetToAttack, _targetTransformToAttack) = Scan(_data.CharacterTransform.position, _data.HitColliderSize, _data.HitColliderOffset, true);

            if (_targetToAttack != null && _targetTransformToAttack != null && _targetToAttack != _currentTargetToAttack)
            {
                _currentTargetToAttack = _targetToAttack;
                OnHasTargetToAttack();
            }
        }

        protected void TargetSearch()
        {
            (_targetToChase, _targetTransformToChase) = Scan(_data.CharacterTransform.position, _data.SensorRadius, Vector2.zero, true);

            if (_targetToChase != null && _targetTransformToChase != null)
            {
                OnTargetDetected();
            }
        }

        protected void OnTargetDetected()
        {
            TargetDetected?.Invoke();
        }

        protected void OnHasTargetToAttack()
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

            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, direction, 0);

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
