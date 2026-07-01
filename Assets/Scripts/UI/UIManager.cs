/// <summary>
/// This script manages the transitions between HUD states.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{

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
    private Canvas menuUI;

    private UIState state;
    private UIState prevState;
    public UIState State { get => state; }

    void Start()
    {
        interactUI = Player.Instance.gameObject.GetComponentInChildren<Canvas>();
        shopUI = ShopUI.Instance.gameObject.GetComponent<Canvas>();
        mapUI = MapUI.Instance.gameObject.GetComponent<Canvas>();
        menuUI = MenuUI.Instance.gameObject.GetComponent<Canvas>();

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
        menuUI.enabled = (state == UIState.MENU);
        Cursor.lockState = (state == UIState.PLAY || state == UIState.MAP ? CursorLockMode.Locked : CursorLockMode.None);
        Cursor.visible = (state == UIState.SHOP || state == UIState.MENU);
    }

    public void SwitchState(UIState newState)
    {
        prevState = (state == UIState.SHOP ? UIState.SHOP : UIState.PLAY);
        state = newState;
        if (state != UIState.PLAY && state != UIState.MAP)
        {
            Player.Instance.Movement.StopMovement();
        }
        if (state == UIState.SHOP)
        {
            ShopUI.Instance.ShopOpen();
        }
        if (state == UIState.MENU)
        {
            MenuUI.Instance.MenuOpen();
        }
    }

    public void CloseMenu()
    {
        state = prevState;
        if (state == UIState.SHOP)
        {
            ShopUI.Instance.ShopOpen();
        }
        Time.timeScale = 1.0f; // unpause game
    }
}
