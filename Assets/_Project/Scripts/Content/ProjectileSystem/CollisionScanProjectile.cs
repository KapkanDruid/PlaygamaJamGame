using UnityEngine;

namespace Project.Content.ProjectileSystem
{
    public class CollisionScanProjectile : Projectile
    {
        protected override void OnTargetCollision(Collider2D collision, IDamageable damageable, IEntity entity)
        {
            damageable.TakeDamage(Damage);
        }
    }
}