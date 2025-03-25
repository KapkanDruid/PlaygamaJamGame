using UnityEngine;

namespace Project.Content.BuildSystem
{
    [CreateAssetMenu(fileName = "WallConfig", menuName = "_Project/Config/WallConfig")]
    public class WallConfig : ScriptableObject
    {
        [Header("Static values")]
        [SerializeField] private int _placeCardAmount;

        [Header("Runtime-modified values")]
        [SerializeField] private float _maxHealth;

        public float MaxHealth => _maxHealth;
        public int PlaceCardAmount => _placeCardAmount;
    }
}
