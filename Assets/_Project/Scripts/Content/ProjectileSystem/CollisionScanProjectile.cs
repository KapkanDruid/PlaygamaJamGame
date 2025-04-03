using UnityEngine;

namespace Project.Content.ProjectileSystem
{
    public class CollisionScanProjectile : Projectile
    {
        protected override void OnTargetCollision(Collision2D collision, IDamageable damageable)
        {
            damageable.TakeDamage(Damage);
        }
    }
}