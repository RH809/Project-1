using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Slider healthbar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;
    
    void Start()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = Player.Instance.Health.MaxHealth;
        healthbar.value = Player.Instance.Health.CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = Player.Instance.Health.CurrentHealth;
        healthText.text = Player.Instance.Health.CurrentHealth.ToString() + "/" + Player.Instance.Health.MaxHealth.ToString();
    }
}
