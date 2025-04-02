namespace Project.Content.BuildSystem
{
    public class DirectProjectileData : IDirectProjectileData
    {
        private EntityFlags[] _enemyFlag;
        private float _damage;
        private float _speed;
        private float _lifeTime;

        public EntityFlags[] EnemyFlag { get => _enemyFlag; set => _enemyFlag = value; }
        public float Damage { get => _damage; set => _damage = value; }
        public float Speed { get => _speed; set => _speed = value; }
        public float LifeTime { get => _lifeTime; set => _lifeTime = value; }
    }
}