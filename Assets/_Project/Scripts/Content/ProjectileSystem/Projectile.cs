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
        [SerializeField] private EntityFlags[] _enemyFlag;

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

        private void OnEnable()
        {
            IsProjectileDisposed = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsProjectileDisposed)
                return;
            if (collision.gameObject.TryGetComponent(out IEntity entity))
            {
                Flags flags = entity.ProvideComponent<Flags>();

                if (flags == null)
                    return;

                bool isEnemy = false;

                for (int j = 0; j < _enemyFlag.Length; j++)
                {
                    if (flags.Contain(_enemyFlag[j]))
                    {
                        isEnemy = true;
                        break;
                    }
                }

                if (!isEnemy)
                    return;
                IDamageable damageable = entity.ProvideComponent<IDamageable>();

                OnTargetCollision(collision, damageable, entity);

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

        public void PrepareProjectile(float damage, EntityFlags[] enemyFlags)
        {
            _damage = damage;
            _enemyFlag = enemyFlags;
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
        protected virtual void OnAnyCollision(Collider2D collision) { }
        protected virtual void OnOtherCollision(Collider2D collision) { }
        protected virtual void OnTargetCollision(Collider2D collision, IDamageable damageable, IEntity entity) { }
    }
}