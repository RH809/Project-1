/// <summary>
/// This script handles the UI behavior of the pause menu.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UIManager;

public class MenuUI : Singleton<MenuUI>
{
    enum Page {
        MENU,
        SETTINGS,
        HOW_TO_PLAY
    };

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject howToPlayPanel;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Button menuCloseButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button howToPlayBackButton;

    [SerializeField] private Settings settings;

    private Page currentPage;

    protected override void Awake()
    {
        base.Awake();

        settingsButton.onClick.AddListener(() => {
            currentPage = Page.SETTINGS;
            settings.OpenSettings();
        });
        howToPlayButton.onClick.AddListener(() => currentPage = Page.HOW_TO_PLAY);
        quitButton.onClick.AddListener(QuitGame);
        menuCloseButton.onClick.AddListener(() => UIManager.Instance.CloseMenu());
        settingsBackButton.onClick.AddListener(() => currentPage = Page.MENU);
        howToPlayBackButton.onClick.AddListener(() => currentPage = Page.MENU);

        currentPage = Page.MENU;
    }

    void OnEnable()
    {
        UIManager.Instance.Input.UIControls.Escape.performed += OnEscapePerformed;
    }

    void OnDisable()
    {
        UIManager.Instance.Input.UIControls.Escape.performed -= OnEscapePerformed;
    }

    void Update()
    {
        menuPanel.SetActive(currentPage == Page.MENU);
        settingsPanel.SetActive(currentPage == Page.SETTINGS);
        howToPlayPanel.SetActive(currentPage == Page.HOW_TO_PLAY);
    }

    public void MenuOpen()
    {
        currentPage = Page.MENU;
        Time.timeScale = 0f; // pause game
    }

    public void OnEscapePerformed(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.GameOver) return;
        if (UIManager.Instance.State == UIState.MENU)
        {
            if (currentPage == Page.MENU)
            {
                UIManager.Instance.CloseMenu();
            }
            else
            {
                currentPage = Page.MENU;
            }
        }
        else
        {
            UIManager.Instance.SwitchState(UIState.MENU);
        }
    }

    void QuitGame()
    {
        Time.timeScale = 1.0f; // unpause right before quit
        Debug.Log("Quitting game.");
        SceneManager.LoadScene(0);
    }

}
