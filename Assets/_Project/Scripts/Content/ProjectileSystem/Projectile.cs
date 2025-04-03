using UnityEngine;

namespace Project.Content.ProjectileSystem
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Projectile : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField, Min(0f)] private float _damage = 10f;
        [SerializeField] private ProjectileDisposeType _disposeType = ProjectileDisposeType.OnAnyCollision;

        [Header("Rigidbody")]
        [SerializeField] private Rigidbody2D _projectileRigidbody;

        [Header("Effect On Destroy")]
        [SerializeField] private bool _spawnEffectOnDestroy = true;
        [SerializeField] private ParticleSystem _effectOnDestroyPrefab;
        [SerializeField, Min(0f)] private float _effectOnDestroyLifetime = 2f;

        public bool IsProjectileDisposed { get; private set; }
        public float Damage => _damage;
        public ProjectileDisposeType DisposeType => _disposeType;
        public Rigidbody2D Rigidbody => _projectileRigidbody;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsProjectileDisposed)
                return;
            
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                OnTargetCollision(collision, damageable);

                if (_disposeType == ProjectileDisposeType.OnTargetCollision)
                {
                    DisposeProjectile();
                }
            }
            else
            {
                OnOtherCollision(collision);
            }
            
            OnAnyCollision(collision);
            
            if (_disposeType == ProjectileDisposeType.OnAnyCollision)
            {
                DisposeProjectile();
            }
        }

        public void DisposeProjectile()
        {
            OnProjectileDispose();
            
            SpawnEffectOnDestroy();

            gameObject.SetActive(false);
            
            IsProjectileDisposed = true;
        }

        private void SpawnEffectOnDestroy()
        {
            if (_spawnEffectOnDestroy == false)
                return;
            
            var effect = Instantiate(_effectOnDestroyPrefab, transform.position, _effectOnDestroyPrefab.transform.rotation);
            
            Destroy(effect.gameObject, _effectOnDestroyLifetime);
        }

        protected virtual void OnProjectileDispose() { }
        protected virtual void OnAnyCollision(Collision2D collision) { }
        protected virtual void OnOtherCollision(Collision2D collision) { }
        protected virtual void OnTargetCollision(Collision2D collision, IDamageable damageable) { }
    }
}