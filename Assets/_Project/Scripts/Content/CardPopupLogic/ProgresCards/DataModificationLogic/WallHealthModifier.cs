using UnityEngine;

namespace Project.Content.UI.DataModification
{
    [CreateAssetMenu(fileName = "WallHealthModifier", menuName = "_Project/ModificationConfig/WallHealthModifier")]
    public class WallHealthModifier : DataModifierConfig
    {
        [SerializeField] private float _addValue;

        public override void ExecuteProgress()
        {
            SceneData.WallDynamicData.BuildingMaxHealth.Value += _addValue;
        }

        public override float GetModifierValue()
        {
            return _addValue;
        }
    }
}
