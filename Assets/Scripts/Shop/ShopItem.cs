/// <summary>
/// This scriptable object stores the info of a shop item.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Item", menuName = "Scriptable Objects/Shop Item")]
public class ShopItem : ScriptableObject
{
    public enum ShopItemType
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

    public ShopItemType shopItem;
    public Sprite image;
    public string itemName;
    public int price;
    public int priceIncrement;
    public int purchaseCap; // -1 means no purchase cap
    public string description;

    private int purchases = 0;
    [HideInInspector] public bool reachedCap { get => purchaseCap != -1 && purchases == purchaseCap; }

    public virtual void Purchase()
    {
        Player.Instance.Bank.RemoveMoney(price);
        purchases++;
        price += priceIncrement;
    }
}
