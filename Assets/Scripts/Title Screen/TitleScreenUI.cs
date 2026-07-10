/// <summary>
/// This script handles the behavior of the UI in the title screen scene.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    private TitleScreenInput input;

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject howToPlayPanel;

    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Button settingsCloseButton;
    [SerializeField] private Button howToPlayCloseButton;

    void Awake()
    {
        input = new TitleScreenInput();
        Debug.Log(input);

        playButton.onClick.AddListener(Play);
        settingsButton.onClick.AddListener(() => settingsPanel.SetActive(true));
        howToPlayButton.onClick.AddListener(() =>  howToPlayPanel.SetActive(true));
        quitButton.onClick.AddListener(QuitGame);

        settingsCloseButton.onClick.AddListener(() => settingsPanel.SetActive(false));
        howToPlayCloseButton.onClick.AddListener(() => howToPlayPanel.SetActive(false));

        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    void OnEnable()
    {
        input.Enable();
        input.TitleScreen.Escape.performed += OnEscapePerformed;
    }

    void OnDisable()
    {
        input.TitleScreen.Escape.performed -= OnEscapePerformed;
        input.Disable();
    }

    public void OnEscapePerformed(InputAction.CallbackContext ctx)
    {
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    void Play()
    {
        SceneManager.LoadScene(1);
    }

    void QuitGame()
    {
        // Closes the built application
        Application.Quit();

        // Stops the play mode in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
