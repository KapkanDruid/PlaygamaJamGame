using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Project.Content.BuildSystem
{
    public class DirectProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidBody;

        private EntityFlags _enemyFlag;
        private float _damage;
        private float _speed;
        private float _lifeTime;
        private float _elapsedTime;

        private Vector2 _moveDirection;
        private Vector3 _lastPosition;

        public void Prepare(Vector2 startPosition, Vector2 moveDirection, IDirectProjectileData directProjectileData)
        {
            _enemyFlag = directProjectileData.EnemyFlag;
            _damage = directProjectileData.Damage;
            _speed = directProjectileData.Speed;
            _lifeTime = directProjectileData.LifeTime;

            _moveDirection = moveDirection;
            transform.position = startPosition;
            _lastPosition = startPosition;

            transform.right = moveDirection;
            _elapsedTime = 0;
        }
         
        private void FixedUpdate()
        {
            Move();

            if (_elapsedTime > _lifeTime)
            {
                gameObject.SetActive(false);
            }
        }

        private void Move()
        {
            _elapsedTime += Time.deltaTime;

            Vector2 nextPosition = _rigidBody.position + (_moveDirection.normalized * _speed * Time.deltaTime);

            _rigidBody.MovePosition(nextPosition);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IEntity entity))
            {
                Flags flags = entity.ProvideComponent<Flags>();

                if (flags == null)
                    return;

                if (!flags.Contain(_enemyFlag))
                    return;

                IDamageable damageable = entity.ProvideComponent<IDamageable>();

                if (damageable == null)
                    return;

                damageable.TakeDamage(_damage, () => gameObject.SetActive(false));
            }
        }
    }
}