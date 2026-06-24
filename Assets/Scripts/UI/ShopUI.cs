/// <summary>
/// This script manages the UI for the shop.
/// </summary>
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    public enum ShopItem
    {
        SWORD_DAMAGE,
        SWORD_ATTACK_SPEED,
        SWORD_CRIT_CHANCE,
        GUN_DAMAGE,
        GUN_ATTACK_SPEED,
        GUN_CRIT_CHANCE,
        REPAIR_TOOL,
        GRENADE,
        HEALTH_POTION
    };

    [SerializeField] private Button swordDamage;
    [SerializeField] private Button swordAttackSpeed;
    [SerializeField] private Button swordCritChance;

    [SerializeField] private Button close;

    [SerializeField] private Image buyImage;
    [SerializeField] private TextMeshProUGUI buyItemText;
    [SerializeField] private TextMeshProUGUI buyPriceText;
    [SerializeField] private TextMeshProUGUI buyDescription;

    [SerializeField] private Image swordImage;
    [SerializeField] private Image gunImage;
    [SerializeField] private Image repairToolImage;
    [SerializeField] private Image grenadeImage;
    [SerializeField] private Image potionImage;

    private Button selectedButton;
    private ShopItem selectedItem;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        Player.Instance.InputManager.Controls.Player.Interact.performed += Close;
    }

    void OnDisable()
    {
        Player.Instance.InputManager.Controls.Player.Interact.performed -= Close;
    }

    void Start()
    {
        swordDamage.onClick.AddListener(() => selectItem(ShopItem.SWORD_DAMAGE, swordDamage));
        swordAttackSpeed.onClick.AddListener(() => selectItem(ShopItem.SWORD_ATTACK_SPEED, swordAttackSpeed));
        swordCritChance.onClick.AddListener(() => selectItem(ShopItem.SWORD_CRIT_CHANCE, swordCritChance));

        close.onClick.AddListener(() => UIManager.Instance.SwitchState(UIManager.UIState.PLAY));

        swordDamage.Select();
    }

    void Update()
    {
    
    }

    void selectItem(ShopItem item, Button button)
    {
        selectedButton = button;
        selectedItem = item;
        Debug.Log("Selected: " + selectedItem);
    }

    void Buy()
    {
        selectedButton.Select();
    }

    void Close(InputAction.CallbackContext ctx)
    {
        UIManager.Instance.SwitchState(UIManager.UIState.PLAY);
    }
}
