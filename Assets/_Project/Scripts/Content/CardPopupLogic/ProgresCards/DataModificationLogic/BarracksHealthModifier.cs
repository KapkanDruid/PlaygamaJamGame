using Project.Content.BuildSystem;
using UnityEngine;

namespace Project.Content.UI.DataModification
{
    [CreateAssetMenu(fileName = "BarracksHealthModifier", menuName = "_Project/ModificationConfig/BarracksHealthModifier")]
    public class BarracksHealthModifier : DataModifierConfig
    {
        [SerializeField] private BarracksType _barracksType;
        [SerializeField] private float _addValue;

        public override void ExecuteProgress()
        {
            SceneData.BarrackDynamicData[_barracksType].BuildingMaxHealth.Value += _addValue;
        }

        public override float GetModifierValue()
        {
            return _addValue;
        }
    }
}
