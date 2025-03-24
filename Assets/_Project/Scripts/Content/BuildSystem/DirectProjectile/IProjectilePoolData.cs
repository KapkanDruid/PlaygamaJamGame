namespace Project.Content.BuildSystem
{
    public interface IProjectilePoolData
    {
        public DirectProjectile ProjectilePrefab { get; }
        public int ProjectilePoolCount { get; }
    }
}