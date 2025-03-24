using Project.Content.BuildSystem;
using UnityEngine;

namespace Project.Content.UI.DataModification
{
    [CreateAssetMenu(fileName = "TurretRadiusModifier", menuName = "_Project/ModificationConfig/TurretRadiusModifier")]
    public class TurretRadiusModifier : DataModifierConfig
    {
        [SerializeField] private TurretType _turretType;
        [SerializeField] private float _addValue;

        public override void ExecuteProgress()
        {
            SceneData.TurretDynamicData[_turretType].SensorRadius.Value += _addValue;
        }
    }
}
