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
    private UIState prevState;
    private UIState prevPrevState;
    public UIState State { get => state; }

    protected override void Awake()
    {
        base.Awake();
        Input = new UIInput();
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
        if (!(state == UIState.BOOSTS && newState == UIState.MENU))
        { // don't override previous state if going from boosts to menu
            prevState = (state == UIState.SHOP ? UIState.SHOP : UIState.PLAY);
            prevPrevState = prevState;
        }
        else
        { // keep track of two states back for case of pausing during boost UI
            prevPrevState = prevState;
            prevState = UIState.BOOSTS;
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
        state = prevState;
        prevState = prevPrevState;
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
