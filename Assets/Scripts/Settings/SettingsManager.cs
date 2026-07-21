/// <summary>
/// This script manages the settings for the game.
/// </summary>
using JetBrains.Annotations;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    public enum Difficulty {
        EASY,
        MEDIUM,
        HARD
    };

    [SerializeField] private bool defaultVignetteSetting;
    [SerializeField] private bool defaultMoneyPopupSetting;
    [SerializeField] private bool defaultDeathParticlesSetting;
    [SerializeField] private bool defaultHealthbarValuesSetting;
    [SerializeField] private int defaultCrosshairSize;
    [SerializeField] private float defaultLookSensitivity;
    [SerializeField] private Difficulty defaultDifficulty;
    public int DefaultCrosshairSize { get => defaultCrosshairSize; }

    [SerializeField] private int minCrosshairSize = 20;
    public int MinCrosshairSize { get => minCrosshairSize; }
    [SerializeField] private int maxCrosshairSize = 80;
    public int MaxCrosshairSize { get => maxCrosshairSize; }

    public float DefaultLookSensitivity { get => defaultLookSensitivity; }

    [SerializeField] private float minLookSensitivity = 0.01f;
    public float MinLookSensitivity { get => minLookSensitivity; }
    [SerializeField] private float maxLookSensitivity = 4.0f;
    public float MaxLookSensitivity { get => maxLookSensitivity; }

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

    private Difficulty difficulty;
    public Difficulty GameDifficulty { get => difficulty; }

    

    private float lookSensitivity;
    public float LookSensitivity { get => lookSensitivity; }
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
        lookSensitivity = defaultLookSensitivity;
        difficulty = defaultDifficulty;
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

    public void SetLookSensitivity(float lookSensitivity)
    {
        this.lookSensitivity = lookSensitivity;
    }

    public void NextDifficulty()
    {
        if (difficulty == Difficulty.EASY)
        {
            difficulty = Difficulty.MEDIUM;
        }
        else if (difficulty == Difficulty.MEDIUM)
        {
            difficulty = Difficulty.HARD;
        }
        else
        {
            difficulty = Difficulty.EASY;
        }
    }

}
