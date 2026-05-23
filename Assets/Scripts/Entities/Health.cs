/// <summary>
/// This script handles the health for all of the different entities.
/// </summary>

using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int baseRegen;
    [SerializeField] private float regenRate;

    private int currentHealth;
    private float regenTimer;

    public bool IsAlive { get => currentHealth > 0; }

    public static event Action<HealthContext> OnHeal;
    public static event Action<HealthContext> OnTakeDamage;
    public static event Action<HealthContext> OnDie;

    private HealthContext healthContext;
    void Start()
    {
        currentHealth = maxHealth;
        regenTimer = regenRate;

        healthContext.target = gameObject;
    }

    void Update()
    {
        // handle periodic regen
        regenTimer -= Time.deltaTime;
        if (currentHealth > 0 && regenTimer <= 0) {
            regenTimer = regenRate;
            currentHealth = Mathf.Min(currentHealth + baseRegen, maxHealth);
        }
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    /// <summary>
    /// Heals the entity by the given amount.
    /// </summary>
    /// <param name="healAmount">The heal amount</param>
    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        healthContext.source = gameObject;
        OnHeal?.Invoke(healthContext);
    }


    /// <summary>
    /// Deals the given amount of damage to the entity and kills it if applicable.
    /// </summary>
    /// <param name="damageAmount">The damage amount</param>
    /// <param name="attacker">The source of the damage</param>
    public void TakeDamage(int damageAmount, GameObject attacker)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        healthContext.source = attacker;
        OnTakeDamage?.Invoke(healthContext);
        if (currentHealth == 0)
        {
            OnDie?.Invoke(healthContext);
        }
    }

    /// <summary>
    /// Respawns the entity by restoring its health.
    /// </summary>
    public void Respawn()
    {
        currentHealth = maxHealth;
        regenTimer = regenRate;
    }   
    
}

/// <summary>
/// Struct for passing information about who is receiving and dealing the damage.
/// </summary>
public struct HealthContext
{
    public GameObject source;
    public GameObject target;
}
