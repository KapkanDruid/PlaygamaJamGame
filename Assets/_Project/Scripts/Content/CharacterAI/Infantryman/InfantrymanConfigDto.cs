using System;

namespace Project.Content.CharacterAI.Infantryman
{
    [Serializable]
    public class InfantrymanConfigDto
    {
        public int Damage;
        public float Health;
        public float Speed;
        public float AttackCooldown;
        public float AttackRange;
        public float SensorRadius;
        public AllyEntityType Type;
    }
}