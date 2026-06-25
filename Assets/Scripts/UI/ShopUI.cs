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

    [SerializeField] private Button swordDamage;
    [SerializeField] private Button swordAttackSpeed;
    [SerializeField] private Button swordCritChance;

    [SerializeField] private Button close;
    [SerializeField] private Button buy;

    [SerializeField] private Image buyImage;
    [SerializeField] private TextMeshProUGUI buyItemText;
    [SerializeField] private TextMeshProUGUI buyPriceText;
    [SerializeField] private TextMeshProUGUI buyDescription;

    [SerializeField] private ShopItemInfo swordDamageInfo;
    [SerializeField] private ShopItemInfo swordAttackSpeedInfo;
    [SerializeField] private ShopItemInfo swordCritChanceInfo;

    private Button selectedButton;
    private ShopItemInfo selectedItem;
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
        swordDamage.onClick.AddListener(() => selectItem(swordDamageInfo, swordDamage));
        swordAttackSpeed.onClick.AddListener(() => selectItem(swordAttackSpeedInfo, swordAttackSpeed));
        swordCritChance.onClick.AddListener(() => selectItem(swordCritChanceInfo, swordCritChance));

        close.onClick.AddListener(() => UIManager.Instance.SwitchState(UIManager.UIState.PLAY));
        buy.onClick.AddListener(Buy);

        selectItem(swordDamageInfo, swordDamage);
    }

    void Update()
    {
        buy.interactable = !selectedItem.reachedCap; // TODO: based on player's money
    }

    void selectItem(ShopItemInfo item, Button button)
    {
        selectedButton = button;
        selectedItem = item;

        buyImage.sprite = selectedItem.image;
        buyItemText.text = selectedItem.name;
        if (selectedItem.reachedCap)
        {
            buyPriceText.text = "Maxed";
            buyDescription.text = "";
        }
        else
        {
            buyPriceText.text = "$" + selectedItem.price.ToString();
            buyDescription.text = selectedItem.description;
        }

        Debug.Log("Selected: " + selectedItem);
    }

    void Buy()
    {
        selectedItem.Purchase();
        if (selectedItem.reachedCap)
        {
            buyPriceText.text = "Maxed";
            buyDescription.text = "";
            TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = buttonText.text.Substring(0, buttonText.text.IndexOf('$')) + "Maxed)";
            buy.interactable = false;
        }
        selectedButton.Select();
    }

    public void ShopOpen()
    {
        selectedButton.Select();
    }

    void Close(InputAction.CallbackContext ctx)
    {
        UIManager.Instance.SwitchState(UIManager.UIState.PLAY);
    }
}
