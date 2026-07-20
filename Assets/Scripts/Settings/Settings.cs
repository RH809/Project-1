/// <summary>
/// This script handles the behavior of the settings menu.
/// </summary>
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle vignetteToggle;
    [SerializeField] private Toggle moneyPopupToggle;
    [SerializeField] private Toggle deathParticlesToggle;
    [SerializeField] private Toggle healthbarValuesToggle;

    [SerializeField] private Slider crosshairSlider;
    [SerializeField] private TextMeshProUGUI crosshairValue;
    [SerializeField] private TextMeshProUGUI crosshair;
    [SerializeField] private Button resetCrosshairButton;

    [SerializeField] private Slider lookSensitivitySlider;
    [SerializeField] private TextMeshProUGUI lookSensitivityValue;
    [SerializeField] private Button resetLookSensitivityButton;

    [SerializeField] private Button difficultyButton;
    [SerializeField] private TextMeshProUGUI difficultyText;

    void Start()
    {
        crosshairSlider.minValue = SettingsManager.Instance.MinCrosshairSize;
        crosshairSlider.maxValue = SettingsManager.Instance.MaxCrosshairSize;

        lookSensitivitySlider.minValue = SettingsManager.Instance.MinLookSensitivity;
        lookSensitivitySlider.maxValue = SettingsManager.Instance.MaxLookSensitivity;

        vignetteToggle.onValueChanged.AddListener(value => SettingsManager.Instance.ToggleVignette(value));
        moneyPopupToggle.onValueChanged.AddListener(value => SettingsManager.Instance.ToggleMoneyPopup(value));
        deathParticlesToggle.onValueChanged.AddListener(value => SettingsManager.Instance.ToggleDeathParticles(value));
        healthbarValuesToggle.onValueChanged.AddListener(value => SettingsManager.Instance.ToggleHealthbarValues(value));

        crosshairSlider.onValueChanged.AddListener(value => UpdateCrosshairSettings((int)value));
        resetCrosshairButton.onClick.AddListener(() => UpdateCrosshairSettings(SettingsManager.Instance.DefaultCrosshairSize));

        lookSensitivitySlider.onValueChanged.AddListener(value => UpdateLookSensitivitySettings(value));
        resetLookSensitivityButton.onClick.AddListener(() => UpdateLookSensitivitySettings(SettingsManager.Instance.DefaultLookSensitivity));

        if (difficultyButton != null)
        {
            difficultyButton.onClick.AddListener(() => {
                SettingsManager.Instance.NextDifficulty();
                SetDifficultyText();
            });
        }
    }

    public void OpenSettings()
    {
        vignetteToggle.isOn = SettingsManager.Instance.ShowVignette;
        moneyPopupToggle.isOn = SettingsManager.Instance.ShowMoneyPopup;
        deathParticlesToggle.isOn = SettingsManager.Instance.ShowDeathParticles;
        healthbarValuesToggle.isOn = SettingsManager.Instance.ShowHealthbarValues;
        crosshairSlider.value = SettingsManager.Instance.CrosshairSize;
        crosshair.fontSize = crosshairSlider.value;
        crosshairValue.text = ((int)crosshairSlider.value).ToString();
        resetCrosshairButton.interactable = crosshairSlider.value != SettingsManager.Instance.DefaultCrosshairSize;
        lookSensitivitySlider.value = SettingsManager.Instance.LookSensitivity;
        lookSensitivityValue.text = lookSensitivitySlider.value.ToString("0.##");
        resetLookSensitivityButton.interactable = lookSensitivitySlider.value != SettingsManager.Instance.DefaultLookSensitivity;
        SetDifficultyText();
    }

    void UpdateCrosshairSettings(int value)
    {
        crosshairSlider.value = value;
        crosshair.fontSize = value;
        crosshairValue.text = value.ToString();
        SettingsManager.Instance.SetCrosshairSize(value);
        resetCrosshairButton.interactable = crosshairSlider.value != SettingsManager.Instance.DefaultCrosshairSize;
    }

    void UpdateLookSensitivitySettings(float value)
    {
        lookSensitivitySlider.value = value;
        lookSensitivityValue.text = value.ToString("0.##");
        SettingsManager.Instance.SetLookSensitivity(value);
        resetLookSensitivityButton.interactable = lookSensitivitySlider.value != SettingsManager.Instance.DefaultLookSensitivity;
    }

    void SetDifficultyText()
    {
        switch (SettingsManager.Instance.GameDifficulty)
        {
            case SettingsManager.Difficulty.EASY:
                difficultyText.text = "Difficulty: Easy";
                break;
            case SettingsManager.Difficulty.MEDIUM:
                difficultyText.text = "Difficulty: Medium";
                break;
            case SettingsManager.Difficulty.HARD:
                difficultyText.text = "Difficulty: Hard";
                break;
        }
    }


}
