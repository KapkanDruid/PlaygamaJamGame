using Project.Content.BuildSystem;
using UnityEngine;

namespace Project.Content.UI.DataModification
{
    [CreateAssetMenu(fileName = "BarracksUnitHealthModifier", menuName = "_Project/ModificationConfig/BarracksUnitHealthModifier")]
    public class BarracksUnitHealthModifier : DataModifierConfig
    {
        [SerializeField] private BarracksType _barracksType;
        [SerializeField] private float _addValue;

        public override void ExecuteProgress()
        {
            SceneData.BarrackDynamicData[_barracksType].UnitHealthModifier += _addValue;
        }

        public override float GetModifierValue()
        {
            return _addValue;
        }
    }
}
