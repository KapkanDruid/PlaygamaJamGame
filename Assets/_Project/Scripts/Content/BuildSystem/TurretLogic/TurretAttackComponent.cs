using Cysharp.Threading.Tasks;
using Project.Content.ObjectPool;
using Project.Content.ProjectileSystem;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class TurretAttackComponent : ITickable
    {
        private readonly IProjectileTypeData _poolData;
        private readonly ITurretShootData _shootData;

        private readonly GizmosDrawer _gizmosDrawer;
        private readonly Transform _localTransform;
        private readonly Transform _shootPoint;
        private readonly PauseHandler _pauseHandler;
        private readonly FiltrablePoolsHandler _poolsHandler;

        private ClosestTargetSensorFilter _sensorFilter;
        private Transform _targetTransform;
        private TargetSensor _sensor;

        private CancellationToken _cancellationToken;

        private bool _isReadyToShoot;
        private bool _isRotating;
        private bool _isActive;
        private float _shootTimer;

        private Predicate<SimpleProjectile> ProjectilePredicate 
            => item => item.ProjectileType == _poolData.ProjectileType && item.gameObject.activeInHierarchy == false;

        public TurretAttackComponent(IProjectileTypeData projectilePoolData,
                                     ITurretShootData shootData,
                                     CancellationToken cancellationToken,
                                     GizmosDrawer gizmosDrawer,
                                     PauseHandler pauseHandler,
                                     FiltrablePoolsHandler poolsHandler)
        {
            _poolData = projectilePoolData;
            _shootData = shootData;
            _cancellationToken = cancellationToken;
            _gizmosDrawer = gizmosDrawer;
            _localTransform = _shootData.RotationObject;
            _shootPoint = _shootData.ShootPoint;
            _pauseHandler = pauseHandler;
            _poolsHandler = poolsHandler;
        }

        public void Initialize()
        {
            _isReadyToShoot = true;
            _sensor = new TargetSensor(_shootData.SensorData, Color.red);
            _sensorFilter = new ClosestTargetSensorFilter(_localTransform);
            _gizmosDrawer.AddGizmosDrawer(_sensor);
            _isActive = true;
        }

        public void Tick()
        {
            if (_pauseHandler.IsPaused)
                return;

            if (!_isActive)
                return;

            HandleTarget();

            if (_targetTransform == null)
                return;

            HandleRotation();
            Shoot();
            UpdateTimer();
        }

        private void HandleTarget()
        {
            if (_targetTransform == null)
            {
                if (!_sensor.TryGetTarget(out IEntity entity, out Transform targetTransform, _sensorFilter))
                    return;

                _targetTransform = targetTransform;
            }
            else
            {
                if (_targetTransform.gameObject.activeInHierarchy)
                    return;

                _targetTransform = null;
            }
        }

        private void HandleRotation()
        {
            Quaternion startRotation = _localTransform.rotation;
            Quaternion targetRotation = DetermineTargetRotation();

            Vector3 rotationDifference = startRotation.eulerAngles - targetRotation.eulerAngles;

            if (Mathf.Abs(rotationDifference.z) > _shootData.RotationThreshold)
                RotateToTargetAsync().Forget();
            else
                _localTransform.rotation = targetRotation;
        }

        private async UniTask RotateToTargetAsync()
        {
            if (_isRotating) 
                return;

            _isRotating = true;

            Quaternion startRotation = _localTransform.rotation;
            Quaternion targetRotation = DetermineTargetRotation();

            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime * _shootData.RotateSpeed;
                _localTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, time);
                try
                {
                    await UniTask.WaitForEndOfFrame(_cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }

            _localTransform.rotation = targetRotation;
            _isRotating = false;
        }

        private Quaternion DetermineTargetRotation()
        {
            Vector3 direction = _targetTransform.position - _localTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, 0, angle);
        }

        private void Shoot()
        {
            if (_isRotating)
                return;

            if (!_isReadyToShoot)
                return;

            _isReadyToShoot = false;

            var projectile = _poolsHandler.GetByPredicate(ProjectilePredicate);

            Vector2 direction = _targetTransform.position - _shootPoint.position;

            projectile.Prepare(_shootPoint.position, direction, _shootData.ProjectileData);

            _shootTimer = _shootData.ReloadTime;
        }

        private void UpdateTimer()
        {
            if (_shootTimer > 0)
            {
                _shootTimer -= Time.deltaTime;
            }
            else
                _isReadyToShoot = true;
        }
    }
}