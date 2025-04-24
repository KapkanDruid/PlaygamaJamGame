namespace Project.Content.ProjectileSystem
{
    public interface ISimpleProjectilePoolData
    {
        public SimpleProjectile ProjectilePrefab { get; }
        public int ProjectilePoolCount { get; }
    }
}