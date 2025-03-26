using Project.Content.BuildSystem;
using UnityEngine;

namespace Project.Content.UI.DataModification
{
    [CreateAssetMenu(fileName = "BarracksUnitDamageModifier", menuName = "_Project/ModificationConfig/BarracksUnitDamageModifier")]
    public class BarracksUnitDamageModifier : DataModifierConfig
    {
        [SerializeField] private BarracksType _barracksType;
        [SerializeField] private float _addValue;

        public override void ExecuteProgress()
        {
            SceneData.BarrackDynamicData[_barracksType].UnitDamageModifier.Value += _addValue;
        }

        public override float GetModifierValue()
        {
            return _addValue;
        }
    }
}
