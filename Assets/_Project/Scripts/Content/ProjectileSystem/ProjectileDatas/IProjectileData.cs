namespace Project.Content.ProjectileSystem
{
    public interface IProjectileData
    {
        public EntityFlags[] EnemyFlag { get; }
        public float Damage { get; }
        public float Speed { get; }
        public float LifeTime { get; }
    }
}