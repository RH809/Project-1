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

    private Canvas interactUI;
    private Canvas shopUI;
    private Canvas mapUI;

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
        interactUI = Player.Instance.gameObject.GetComponentInChildren<Canvas>();
        shopUI = ShopUI.Instance.gameObject.GetComponent<Canvas>();
        mapUI = MapUI.Instance.gameObject.GetComponent<Canvas>();

        state = UIState.PLAY;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        interactUI.enabled = (state == UIState.PLAY);
        mapUI.enabled = (state == UIState.MAP);
        shopUI.enabled = (state == UIState.SHOP);
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
        if (newState == UIState.SHOP)
        {
            ShopUI.Instance.ShopOpen();
        }
    }
}
