/// <summary>
/// This scriptable object stores the info of a shop item.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Item Info", menuName = "Scriptable Objects/Shop Item Info")]
public class ShopItemInfo : ScriptableObject
{
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

    public ShopItem shopItem;
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
        purchases++;
        price += priceIncrement;
    }
}
