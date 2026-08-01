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
    public UIState GetPreviousState { get => previousStates.Count == 0 ? UIState.PLAY : previousStates.Peek(); }
    public UIState State { get => state; }
    private volatile bool shopInStack = false;
    public bool ShopInStack { get => shopInStack; }

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
        Cursor.lockState = (state == UIState.PLAY ? CursorLockMode.Locked : CursorLockMode.None);
        Cursor.visible = (state == UIState.SHOP || state == UIState.MENU || state == UIState.BOOSTS);
    }

    public void SwitchState(UIState newState)
    {
        //Debug.Log(state + " " + newState);
        if (state != UIState.MAP && newState != UIState.PLAY)
        { // don't save map
            previousStates.Push(state);
            if (state == UIState.SHOP)
            {
                shopInStack = true;
            }
        } 
        state = newState;
        if (state == UIState.PLAY)
        {
            previousStates.Clear();
        }
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
            //if (state == UIState.BOOSTS) Debug.Log(BoostsUI.Instance.FinishedFadingOut);
            if (state == UIState.BOOSTS && BoostsUI.Instance.FinishedFadingOut)
            {
                PreviousState();
            }
        }
        //state = prevState;
        //prevState = prevPrevState;
        //Debug.Log("Switching to previous state: " + state);
        if (state == UIState.SHOP)
        {
            ShopUI.Instance.ShopOpen();
            shopInStack = false;
        }
        if (state != UIState.MENU && state != UIState.BOOSTS)
        {
            Time.timeScale = 1.0f; // unpause game
        }
    }
    
    // For handling special cases when switching back from boosts
    public void BoostsPreviousState()
    {
        if (state == UIState.BOOSTS)
        {
            PreviousState();
        }
        else if (state == UIState.MAP)
        {
            Time.timeScale = 1.0f;
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
