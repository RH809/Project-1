/// <summary>
/// This script manages the settings for the game.
/// </summary>
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    [SerializeField] private bool defaultVignetteSetting;
    [SerializeField] private bool defaultMoneyPopupSetting;
    [SerializeField] private bool defaultDeathParticlesSetting;
    [SerializeField] private int defaultCrosshairSize;

    private bool showVignette;
    public bool ShowVignette { get => showVignette; }
    private bool showMoneyPopup;
    public bool ShowMoneyPopup { get => showMoneyPopup; }
    private bool showDeathParticles;
    public bool ShowDeathParticles { get => showDeathParticles; }

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
        crosshairSize = defaultCrosshairSize;
    }

    public void toggleVignette(bool show)
    {
        showVignette = show;
    }

    public void toggleMoneyPopup(bool show)
    {
        showMoneyPopup = show;
    }

    public void toggleDeathParticles(bool show)
    {
        showDeathParticles = show;
    }

    public void SetCrosshairSize(int crosshairSize)
    {
        this.crosshairSize = crosshairSize;
    }

    public void ResetCrosshairSize()
    {
        crosshairSize = defaultCrosshairSize;
    }

}
