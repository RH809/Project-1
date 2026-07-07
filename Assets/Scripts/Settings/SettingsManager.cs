/// <summary>
/// This script manages the settings for the game.
/// </summary>
using JetBrains.Annotations;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    [SerializeField] private bool defaultVignetteSetting;
    [SerializeField] private bool defaultMoneyPopupSetting;
    [SerializeField] private bool defaultDeathParticlesSetting;
    [SerializeField] private bool defaultHealthbarValuesSetting;
    [SerializeField] private int defaultCrosshairSize;
    public int DefaultCrosshairSize { get => defaultCrosshairSize; }

    [SerializeField] private int minCrosshairSize = 20;
    public int MinCrosshairSize { get => minCrosshairSize; }
    [SerializeField] private int maxCrosshairSize = 80;
    public int MaxCrosshairSize { get => maxCrosshairSize; }

    private bool showVignette;
    public bool ShowVignette { get => showVignette; }
    private bool showMoneyPopup;
    public bool ShowMoneyPopup { get => showMoneyPopup; }
    private bool showDeathParticles;
    public bool ShowDeathParticles { get => showDeathParticles; }

    private bool showHealthbarValues;
    public bool ShowHealthbarValues { get => showHealthbarValues; }

    private int crosshairSize;
    public int CrosshairSize { get => crosshairSize; }
    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
            DontDestroyOnLoad(gameObject);

        InitializeSettings();
    }

    private void InitializeSettings()
    {
        showVignette = defaultVignetteSetting;
        showMoneyPopup = defaultMoneyPopupSetting;
        showDeathParticles = defaultDeathParticlesSetting;
        showHealthbarValues = defaultHealthbarValuesSetting;
        crosshairSize = defaultCrosshairSize;
    }

    public void ToggleVignette(bool show)
    {
        showVignette = show;
    }

    public void ToggleMoneyPopup(bool show)
    {
        showMoneyPopup = show;
    }

    public void ToggleDeathParticles(bool show)
    {
        showDeathParticles = show;
    }

    public void ToggleHealthbarValues(bool show)
    {
        showHealthbarValues = show;
    }

    public void SetCrosshairSize(int crosshairSize)
    {
        this.crosshairSize = crosshairSize;
    }

}
