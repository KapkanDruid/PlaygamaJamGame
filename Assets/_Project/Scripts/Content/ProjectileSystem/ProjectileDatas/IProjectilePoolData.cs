namespace Project.Content.ProjectileSystem
{
    public interface IProjectilePoolData
    {
        public SimpleProjectile ProjectilePrefab { get; }
        public int ProjectilePoolCount { get; }
    }
}