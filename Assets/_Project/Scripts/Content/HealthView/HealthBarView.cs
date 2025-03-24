using UnityEngine;
using UnityEngine.UI;

namespace Project.Content
{
    public class HealthBarView : MonoBehaviour, IHealthView
    {
        [SerializeField] private Image _fillBar;

        public void SetHealth(float currentHealth, float maxHealth)
        {
            if (currentHealth == maxHealth)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);

            _fillBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
