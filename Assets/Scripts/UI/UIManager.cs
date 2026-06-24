/// <summary>
/// This script manages the transitions between HUD states.
/// </summary>
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public enum UIState
    {
        PLAY,
        SHOP,
        MAP,
        MENU
    };

    private Canvas interactHUD;
    private Canvas shopHUD;

    private UIState state;
    public UIState State { get => state; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        interactHUD = Player.Instance.gameObject.GetComponentInChildren<Canvas>();
        shopHUD = ShopUI.Instance.gameObject.GetComponent<Canvas>();

        state = UIState.PLAY;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        interactHUD.enabled = (state == UIState.PLAY);
        shopHUD.enabled = (state == UIState.SHOP);
        Cursor.lockState = (state == UIState.PLAY || state == UIState.MAP ? CursorLockMode.Locked : CursorLockMode.None);
        Cursor.visible = (state == UIState.SHOP || state == UIState.MENU);
    }

    public void SwitchState(UIState newState)
    {
        state = newState;
        if (newState != UIState.PLAY && newState != UIState.MAP)
        {
            Player.Instance.Movement.StopMovement();
        }
    }
}
