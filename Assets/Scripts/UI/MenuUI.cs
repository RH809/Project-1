using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UIManager;

public class MenuUI : MonoBehaviour
{
    public static MenuUI Instance;

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

    private Page currentPage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        settingsButton.onClick.AddListener(() => currentPage = Page.SETTINGS);
        howToPlayButton.onClick.AddListener(() => currentPage = Page.HOW_TO_PLAY);
        quitButton.onClick.AddListener(() => Debug.Log("Quit!")); // TODO: Change to quit to title screen
        menuCloseButton.onClick.AddListener(() => UIManager.Instance.CloseMenu());
        settingsBackButton.onClick.AddListener(() => currentPage = Page.MENU);
        howToPlayBackButton.onClick.AddListener(() => currentPage = Page.MENU);

        currentPage = Page.MENU;
    }

    void OnEnable()
    {
        Player.Instance.InputManager.Controls.Player.Escape.performed += OnEscapePerformed;
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

}
