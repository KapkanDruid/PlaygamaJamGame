using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class TurretAttackComponent : ITickable
    {
        private readonly IProjectilePoolData _poolData;
        private readonly ITurretShootData _shootData;

        private readonly GizmosDrawer _gizmosDrawer;
        private readonly Transform _localTransform;
        private readonly Transform _shootPoint;
        private readonly PauseHandler _pauseHandler;

        private ClosestTargetSensorFilter _sensorFilter;
        private Transform _targetTransform;
        private TargetSensor _sensor;

        private CancellationToken _cancellationToken;

        private ObjectPooler<DirectProjectile> _projectilePool;
        private bool _isReadyToShoot;
        private bool _isRotating;
        private bool _isActive;

        public TurretAttackComponent(IProjectilePoolData projectilePoolData, ITurretShootData shootData, CancellationToken cancellationToken, GizmosDrawer gizmosDrawer, PauseHandler pauseHandler)
        {
            _poolData = projectilePoolData;
            _shootData = shootData;
            _cancellationToken = cancellationToken;
            _gizmosDrawer = gizmosDrawer;
            _localTransform = _shootData.RotationObject;
            _shootPoint = _shootData.ShootPoint;
            _pauseHandler = pauseHandler;
        }

        public void Initialize()
        {
            _projectilePool = new ObjectPooler<DirectProjectile>(_poolData.ProjectilePoolCount, "TurretProjectiles", new InstantiateObjectsSimple<DirectProjectile>(_poolData.ProjectilePrefab));
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

            var projectile = _projectilePool.Get();
            projectile.Prepare(_shootPoint.position, _targetTransform.position - _localTransform.position, _shootData.ProjectileData, _pauseHandler);

            RechargeRangeAttack().Forget();
        }

        private async UniTaskVoid RechargeRangeAttack()
        {
            try
            {
                await UniTask.WaitForSeconds(_shootData.ReloadTime, cancellationToken: _cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            _isReadyToShoot = true;
        }
    }
}