namespace Project.Content.CharacterAI
{
    public interface IAllyEntityConfig
    {
        public int Damage { get; }
        public float Speed { get; }
        public float Health { get; }
        public float SensorRadius { get; }
        public float AttackRange { get; }
        public float AttackCooldown { get; }
    }
}