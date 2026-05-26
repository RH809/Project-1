/// <summary>
/// This script handles the healthbar for entities.
/// </summary>

using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;
    [SerializeField] private Vector3 offset;

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
            fill.color = Color.Lerp(minColor, maxColor, ((float)health.CurrentHealth) / health.MaxHealth);
            slider.value = health.CurrentHealth;
            // Update rotation to face player
            transform.rotation = Quaternion.LookRotation(transform.position - Player.Instance.MainCamera.transform.position);
            transform.position = entity.transform.position + offset;
        }
    }
}
