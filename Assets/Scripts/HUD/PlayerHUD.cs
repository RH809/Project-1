/// <summary>
/// This script handles the behavior of the player HUD.
/// </summary>
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Slider healthbar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private Image staminaFill;
    [SerializeField] private Color sprintEnabledColor;
    [SerializeField] private Color sprintDisabledColor;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;
    
    void Start()
    {
        healthbar.minValue = 0;
        healthbar.maxValue = Player.Instance.Health.MaxHealth;
        healthbar.value = Player.Instance.Health.CurrentHealth;
        staminaBar.minValue = 0;
        staminaBar.maxValue = Player.Instance.Stamina.MaxStamina;
        staminaBar.value = Player.Instance.Stamina.CurrentStamina;
        staminaFill.color = sprintEnabledColor;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = Player.Instance.Health.CurrentHealth;
        healthText.text = Player.Instance.Health.CurrentHealth.ToString() + "/" + Player.Instance.Health.MaxHealth.ToString();
        staminaBar.value = Player.Instance.Stamina.CurrentStamina;
        staminaFill.color = (Player.Instance.Stamina.SprintDisabled ? sprintDisabledColor : sprintEnabledColor);
    }
}
