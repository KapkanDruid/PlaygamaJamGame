using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Content.BasicAI
{
    public class CharacterSensor : ITickable
    {
        private readonly CharacterData _data;
        private Transform _targetTransform;
        private IEntity _target;

        private bool _isEnable;

        public bool IsEnable { get => _isEnable; set => _isEnable = value; }
        public IEntity Target => _target;
        public Transform TargetTransform => _targetTransform;

        public event Action TargetDetected;

        public CharacterSensor(CharacterData data)
        {
            _data = data;
            _isEnable = true;
        }

        public void Tick()
        {
            if (_isEnable) 
                TargetSearch();
        }

        private void TargetSearch()
        {
            if (_data == null) 
                return;

            Vector2 origin = _data.CharacterTransform.position;
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

                if (!flags.Contain(_data.EnemyFlag))
                    continue;

                _target = entity;
                _targetTransform = hits[i].transform;

                TargetDetected?.Invoke();
            }
        }
    }
}
