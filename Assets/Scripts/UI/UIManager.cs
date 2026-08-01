/// <summary>
/// This script manages the transitions between HUD states.
/// </summary>
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{

    public enum UIState
    {
        PLAY,
        SHOP,
        MAP,
        BOOSTS,
        MENU
    };

    private Canvas interactUI;
    private Canvas shopUI;
    private Canvas mapUI;
    private Canvas boostsUI;
    private Canvas menuUI;
    private Canvas gameUI;
    
    [SerializeField] private Canvas respawnUI;

    public UIInput Input;

    private UIState state;
    private Stack<UIState> previousStates;
    public UIState State { get => state; }

    protected override void Awake()
    {
        base.Awake();
        Input = new UIInput();
        previousStates = new Stack<UIState>();
    }

    void Start()
    {
        interactUI = Player.Instance.gameObject.GetComponentInChildren<Canvas>();
        shopUI = ShopUI.Instance.gameObject.GetComponent<Canvas>();
        mapUI = MapUI.Instance.gameObject.GetComponent<Canvas>();
        boostsUI = BoostsUI.Instance.gameObject.GetComponent<Canvas>();
        menuUI = MenuUI.Instance.gameObject.GetComponent<Canvas>();
        gameUI = GameManager.Instance.gameObject.GetComponentInChildren<Canvas>();

        state = UIState.PLAY;
        gameUI.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        Input.Enable();
        Health.OnDie += OnPlayerDeath;   
    }

    private void OnDisable()
    {
        Input.Disable();
        Health.OnDie -= OnPlayerDeath;
    }

    void Update()
    {
        if (GameManager.Instance.GameOver) return;
        interactUI.enabled = (state == UIState.PLAY);
        mapUI.enabled = (state == UIState.MAP);
        shopUI.enabled = (state == UIState.SHOP);
        boostsUI.enabled = (state == UIState.BOOSTS);
        menuUI.enabled = (state == UIState.MENU);
        Cursor.lockState = (state == UIState.PLAY || state == UIState.MAP ? CursorLockMode.Locked : CursorLockMode.None);
        Cursor.visible = (state == UIState.SHOP || state == UIState.MENU || state == UIState.BOOSTS);
    }

    public void SwitchState(UIState newState)
    {
        if (state != UIState.MAP)
        { // don't save map
            previousStates.Push(state);
        } 
        state = newState;
        if (state != UIState.PLAY && state != UIState.MAP)
        {
            Player.Instance.Movement.StopMovement();
        }
        if (state == UIState.SHOP)
        {
            ShopUI.Instance.ShopOpen();
        }
        else if (state == UIState.MENU)
        {
            MenuUI.Instance.MenuOpen();
        }
        else if (state == UIState.BOOSTS)
        {
            BoostsUI.Instance.Open();
        }
    }

    public void PreviousState()
    {
        if (previousStates.Count == 0)
        {
            state = UIState.PLAY;
        }
        else
        {
            state = previousStates.Pop();
        }
        //state = prevState;
        //prevState = prevPrevState;
        if (state == UIState.SHOP)
        {
            ShopUI.Instance.ShopOpen();
        }
        if (state != UIState.MENU && state != UIState.BOOSTS)
        {
            Time.timeScale = 1.0f; // unpause game
        }
    }

    public void OnPlayerDeath(HealthContext healthContext)
    {
        if (healthContext.target == Player.Instance.gameObject)
        {
            if (state == UIState.SHOP)
            {
                state = UIState.PLAY;
            }
            previousStates.Clear();
        }
    }

    public void DisableAllUI()
    {
        state = UIState.PLAY;
        interactUI.enabled = false;
        shopUI.enabled = false;
        mapUI.enabled = false;
        boostsUI.enabled = false;
        menuUI.enabled = false;
        respawnUI.enabled = false;
    }
}
