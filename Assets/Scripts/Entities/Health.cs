/// <summary>
/// This script handles the health for all of the different entities.
/// </summary>

using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float baseRegen;
    [SerializeField] private float regenRate;
    [SerializeField] private float hitResetTime;
    [SerializeField] private bool hasHealthbar = true;

    [SerializeField] private GameObject healthbarPrefab;
    private GameObject healthbar;
    [SerializeField] private GameObject mapHealthbar;

    private float currentHealth;
    private float regenTimer;
    private float hitTimer;

    public bool IsAlive { get => currentHealth > 0; }
    public float MaxHealth { get => maxHealth;  }
    public float CurrentHealth { get => currentHealth; }

    public static event Action<HealthContext> OnHeal;
    public static event Action<HealthContext> OnTakeDamage;
    public static event Action<HealthContext> OnDie;
    public static event Action<HealthContext> OnRespawn;

    private HealthContext healthContext;
    void Awake()
    {
        currentHealth = maxHealth;
        regenTimer = regenRate;

        healthContext.target = gameObject;
        if (hasHealthbar)
        {
            // create healthbar
            healthbar = Instantiate(healthbarPrefab);
            healthbar.transform.SetParent(WorldSpaceCanvas.Instance.transform, false);
            healthbar.GetComponent<Healthbar>().Initialize(this, gameObject);
        }
    }

    void Update()
    {
        // handle periodic regen
        regenTimer = Mathf.Max(regenTimer - Time.deltaTime, 0);
        hitTimer = Mathf.Max(hitTimer - Time.deltaTime, 0);
        if (currentHealth > 0 && regenTimer <= 0 && hitTimer <= 0) {
            regenTimer = regenRate;
            currentHealth = Mathf.Min(currentHealth + baseRegen, maxHealth);
        }
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    /// <summary>
    /// Heals the entity by the given amount.
    /// </summary>
    /// <param name="healAmount">The heal amount</param>
    public void Heal(float healAmount)
    {
        if (!IsAlive || GameManager.Instance.GameOver) return;
        Debug.Log($"{gameObject} healed {healAmount} health");
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        healthContext.source = gameObject;
        OnHeal?.Invoke(healthContext);
    }


    /// <summary>
    /// Deals the given amount of damage to the entity and kills it if applicable.
    /// </summary>
    /// <param name="damageAmount">The damage amount</param>
    /// <param name="attacker">The source of the damage</param>
    public void TakeDamage(float damageAmount, GameObject attacker)
    {
        if (!IsAlive || GameManager.Instance.GameOver) return;
        Debug.Log($"{gameObject} took {damageAmount} damage from {attacker}");
        hitTimer = hitResetTime;
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        healthContext.source = attacker;
        OnTakeDamage?.Invoke(healthContext);
        if (currentHealth == 0)
        {
            HideHealthbar();
            OnDie?.Invoke(healthContext);
        }
    }

    /// <summary>
    /// Respawns the entity by restoring its health.
    /// </summary>
    public void Respawn(float maxHealthProportion, bool showHealthbar)
    {
        currentHealth = maxHealth * maxHealthProportion;
        Debug.Log($"{gameObject} respawning to {currentHealth} health");
        regenTimer = regenRate;
        if (showHealthbar)
        {
            ShowHealthBar();
        }
        healthContext.source = gameObject;
        OnRespawn?.Invoke(healthContext);
    }

    /// <summary>
    /// Hides the healthbar (for entities that can respawn after dying)
    /// </summary>
    public void HideHealthbar()
    {
        if (hasHealthbar)
        {
            healthbar.SetActive(false);
            if(mapHealthbar) mapHealthbar.SetActive(false);
        }
    }

    public void ShowHealthBar()
    {
        if (hasHealthbar)
        {
            healthbar.SetActive(true);
            if (mapHealthbar) mapHealthbar.SetActive(true);
        }
    }

    /// <summary>
    /// Destroys the healthbar GameObject (for entities that do not respawn)
    /// </summary>
    public void DestroyHealthbar()
    {
        if (hasHealthbar)
        {
            Destroy(healthbar);
        }
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
