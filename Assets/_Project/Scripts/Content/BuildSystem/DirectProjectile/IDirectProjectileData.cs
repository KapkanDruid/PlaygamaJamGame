namespace Project.Content.BuildSystem
{
    public interface IDirectProjectileData
    {
        public EntityFlags[] EnemyFlag { get; }
        public float Damage { get; }
        public float Speed { get; }
        public float LifeTime { get; }
    }
}