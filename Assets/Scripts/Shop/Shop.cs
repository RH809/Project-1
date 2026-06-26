/// <summary>
/// This script stores the ShopItem/StatUpgrade instances.
/// </summary>
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [SerializeField] private StatUpgrade swordDamageBase;
    [SerializeField] private StatUpgrade swordAttackSpeedBase;
    [SerializeField] private StatUpgrade swordCritChanceBase;

    [SerializeField] private StatUpgrade gunDamageBase;
    [SerializeField] private StatUpgrade gunAttackSpeedBase;
    [SerializeField] private StatUpgrade gunCritChanceBase;

    [SerializeField] private ShopItem repairToolBase;
    [SerializeField] private ShopItem grenadeBase;
    [SerializeField] private ShopItem healthPotionBase;

    [HideInInspector] public StatUpgrade swordDamage;
    [HideInInspector] public StatUpgrade swordAttackSpeed;
    [HideInInspector] public StatUpgrade swordCritChance;

    [HideInInspector] public StatUpgrade gunDamage;
    [HideInInspector] public StatUpgrade gunAttackSpeed;
    [HideInInspector] public StatUpgrade gunCritChance;

    [HideInInspector] public ShopItem repairTool;
    [HideInInspector] public ShopItem grenade;
    [HideInInspector] public ShopItem healthPotion;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        swordDamage = Instantiate(swordDamageBase);
        swordAttackSpeed = Instantiate(swordAttackSpeedBase);
        swordCritChance = Instantiate(swordCritChanceBase);

        gunDamage = Instantiate(gunDamageBase);
        gunAttackSpeed = Instantiate(gunAttackSpeedBase);
        gunCritChance = Instantiate(gunCritChanceBase);

        repairTool = Instantiate(repairToolBase);
        grenade = Instantiate(grenadeBase);
        healthPotion = Instantiate(healthPotionBase);
    }
}
