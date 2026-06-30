using UnityEngine;
using UnityEngine.UI;

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

}
