/// <summary>
/// This script manages the UI for the shop.
/// </summary>
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopUI : Singleton<ShopUI>
{

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private Button swordDamage;
    [SerializeField] private Button swordAttackSpeed;
    [SerializeField] private Button swordCritChance;

    [SerializeField] private Button gunDamage;
    [SerializeField] private Button gunAttackSpeed;
    [SerializeField] private Button gunCritChance;

    [SerializeField] private Button repairTool;
    [SerializeField] private Button grenade;
    [SerializeField] private Button healthPotion;

    [SerializeField] private Button close;
    [SerializeField] private Button buy;

    [SerializeField] private Image buyImage;
    [SerializeField] private TextMeshProUGUI buyItemText;
    [SerializeField] private TextMeshProUGUI buyPriceText;
    [SerializeField] private TextMeshProUGUI buyDescription;

    private Button selectedButton;
    private ShopItem selectedItem;

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
        swordDamage.onClick.AddListener(() => selectItem(Shop.Instance.swordDamage, swordDamage));
        swordAttackSpeed.onClick.AddListener(() => selectItem(Shop.Instance.swordAttackSpeed, swordAttackSpeed));
        swordCritChance.onClick.AddListener(() => selectItem(Shop.Instance.swordCritChance, swordCritChance));

        gunDamage.onClick.AddListener(() => selectItem(Shop.Instance.gunDamage, gunDamage));
        gunAttackSpeed.onClick.AddListener(() => selectItem(Shop.Instance.gunAttackSpeed, gunAttackSpeed));
        gunCritChance.onClick.AddListener(() => selectItem(Shop.Instance.gunCritChance, gunCritChance));

        repairTool.onClick.AddListener(() => selectItem(Shop.Instance.repairTool, repairTool));
        grenade.onClick.AddListener(() => selectItem(Shop.Instance.grenade, grenade));
        healthPotion.onClick.AddListener(() => selectItem(Shop.Instance.healthPotion, healthPotion));

        close.onClick.AddListener(() => UIManager.Instance.SwitchState(UIManager.UIState.PLAY));
        buy.onClick.AddListener(Buy);

        selectItem(Shop.Instance.swordDamage, swordDamage);
    }

    void Update()
    {
        selectedButton.Select();
    }

    void selectItem(ShopItem item, Button button)
    {
        selectedButton = button;
        selectedItem = item;

        buyImage.sprite = selectedItem.image;
        buyItemText.text = selectedItem.itemName;
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
        buy.interactable = !selectedItem.reachedCap && Player.Instance.Bank.Amount >= selectedItem.price;

        Debug.Log("Selected: " + selectedItem.itemName);
    }

    void Buy()
    {
        if (selectedItem.reachedCap)
        {
            selectedButton.Select();
            return;
        }
        selectedItem.Purchase();
        switch (selectedItem.shopItem) {
            case ShopItem.ShopItemType.SWORD_DAMAGE:
            case ShopItem.ShopItemType.SWORD_CRIT_CHANCE:
            case ShopItem.ShopItemType.GUN_DAMAGE:
            case ShopItem.ShopItemType.GUN_CRIT_CHANCE: // Purchase method handles stat upgrades
                break;
            case ShopItem.ShopItemType.SWORD_ATTACK_SPEED:
                playerAnimator.SetFloat("SwordAttackSpeed", playerAnimator.GetFloat("SwordAttackSpeed") + Shop.Instance.swordAttackSpeed.statValueIncrement / 4);
                Player.Instance.Inventory.ReduceSwordCooldown();
                break;
            case ShopItem.ShopItemType.GUN_ATTACK_SPEED:
                playerAnimator.SetFloat("GunAttackSpeed", playerAnimator.GetFloat("GunAttackSpeed") + Shop.Instance.gunAttackSpeed.statValueIncrement / 4);
                Player.Instance.Inventory.ReduceGunCooldown();
                break;
            case ShopItem.ShopItemType.REPAIR_TOOL:
                Player.Instance.Inventory.AddRepairTool();
                break;
            case ShopItem.ShopItemType.GRENADE:
                Player.Instance.Inventory.AddGrenade();
                break;
            case ShopItem.ShopItemType.HEALTH_POTION:
                Player.Instance.Inventory.AddPotion();
                break;
        }
        buyPriceText.text = "$" + selectedItem.price.ToString();
        buyDescription.text = selectedItem.description;
        TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = buttonText.text.Substring(0, buttonText.text.IndexOf('$') + 1) + selectedItem.price.ToString() + ")";
        if (selectedItem.reachedCap)
        {
            buyPriceText.text = "Maxed";
            buyDescription.text = "";
            buttonText.text = buttonText.text.Substring(0, buttonText.text.IndexOf('$')) + "Maxed)";
        }
        buy.interactable = !selectedItem.reachedCap && Player.Instance.Bank.Amount >= selectedItem.price;
        selectedButton.Select();
        Debug.Log("Bought: " + selectedItem.itemName);
    }

    public void ShopOpen()
    {
        selectedButton.Select();
        selectItem(selectedItem, selectedButton);
    }

    void Close(InputAction.CallbackContext ctx)
    {
        if (UIManager.Instance.State == UIManager.UIState.SHOP)
        {
            UIManager.Instance.SwitchState(UIManager.UIState.PLAY);
        }
    }
}
