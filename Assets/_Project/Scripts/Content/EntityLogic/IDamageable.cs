using System;

namespace Project.Content
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, Action callBack = null);
    }
}