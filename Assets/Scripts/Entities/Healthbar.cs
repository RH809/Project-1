/// <summary>
/// This script handles the healthbar for entities.
/// </summary>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;
    [SerializeField] private Vector3 offset;
    [SerializeField] private TextMeshProUGUI healthText;

    private Health health;
    private GameObject entity;

    /// <summary>
    /// Initializes the values of the healthbar based on the given Health component.
    /// </summary>
    /// <param name="health">Health component of the entity</param>
    /// <param name="entity">The entity the healthbar is for</param>
    public void Initialize(Health health, GameObject entity)
    {
        this.health = health;
        this.entity = entity;
        slider.minValue = 0;
        slider.maxValue = health.MaxHealth;
        slider.value = health.MaxHealth;
    }

    void Update()
    {
        if (health)
        {
            // Update color and slide values
            fill.color = Color.Lerp(minColor, maxColor, health.CurrentHealth / health.MaxHealth);
            slider.value = health.CurrentHealth;
            healthText.enabled = SettingsManager.Instance.ShowHealthbarValues;
            healthText.text = health.CurrentHealth.ToString("0.##") + "/" + health.MaxHealth.ToString();
            // Update rotation to face player or respawn camera
            if (Player.Instance.Health.IsAlive)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - Player.Instance.Camera.transform.position);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(transform.position - Respawn.Instance.Camera.transform.position);
            }
            transform.position = entity.transform.position + offset;
        }
    }
}
