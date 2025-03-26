using Project.Content.UI.DataModification;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.Content.UI
{
    public class ModifierIcon : MonoBehaviour
    {
        [SerializeField] private DataModifierConfig _modifierConfig;
        [SerializeField] private TextMeshProUGUI _modifierCountText;

        private int _modifierCount;

        private void Start()
        {
            DataModifierCard.OnModifierApplied += ShowIcon;

            gameObject.SetActive(false);
            _modifierCountText.gameObject.SetActive(false);
        }

        private void ShowIcon(DataModifierConfig modifierConfig)
        {
            if (modifierConfig != _modifierConfig)
                return;

            _modifierCount++;

            if (_modifierCount < 2)
            {
                gameObject.SetActive(true);
                _modifierCountText.gameObject.SetActive(false);
                return;
            }

            _modifierCountText.gameObject.SetActive(true);
            gameObject.SetActive(true);

            _modifierCountText.text = "x" + _modifierCount.ToString();
        }

        private void OnDestroy()
        {
            DataModifierCard.OnModifierApplied -= ShowIcon;
        }
    }
}
