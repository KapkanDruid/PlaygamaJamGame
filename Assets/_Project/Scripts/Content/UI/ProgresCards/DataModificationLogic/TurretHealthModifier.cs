using Project.Content.BuildSystem;
using UnityEngine;

namespace Project.Content.UI.DataModification
{
    [CreateAssetMenu(fileName = "TurretHealthModifier", menuName = "_Project/ModificationConfig/TurretHealthModifier")]
    public class TurretHealthModifier : DataModifierConfig
    {
        [SerializeField] private TurretType _turretType;
        [SerializeField] private float _addValue;

        public override void ExecuteProgress()
        {
            SceneData.TurretDynamicData[_turretType].MaxHealth.Value += _addValue;
        }
    }
}
