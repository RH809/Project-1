/// <summary>
/// This script handles the behavior of the healthbars on the map.
/// </summary>
using UnityEngine;
using UnityEngine.UI;

public class MapHealthbar : MonoBehaviour
{
    [SerializeField] private Health constructHealth;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = constructHealth.MaxHealth;
        slider.value = constructHealth.MaxHealth;
    }
    void Update()
    {
        fill.color = Color.Lerp(minColor, maxColor, constructHealth.CurrentHealth / constructHealth.MaxHealth);
        slider.value = constructHealth.CurrentHealth;
    }
}
