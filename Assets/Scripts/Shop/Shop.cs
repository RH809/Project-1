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

    [HideInInspector] public StatUpgrade swordDamage;
    [HideInInspector] public StatUpgrade swordAttackSpeed;
    [HideInInspector] public StatUpgrade swordCritChance;

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
    }
}
