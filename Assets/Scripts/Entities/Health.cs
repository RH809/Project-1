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
    [SerializeField] private bool hasHealthbar = true;

    [SerializeField] private GameObject healthbarPrefab;
    private GameObject healthbar;
    private Transform worldSpaceCanvas;

    private int currentHealth;
    private float regenTimer;

    public bool IsAlive { get => currentHealth > 0; }
    public int MaxHealth { get => maxHealth;  }
    public int CurrentHealth { get => currentHealth; }

    public static event Action<HealthContext> OnHeal;
    public static event Action<HealthContext> OnTakeDamage;
    public static event Action<HealthContext> OnDie;

    private HealthContext healthContext;
    void Start()
    {
        currentHealth = maxHealth;
        regenTimer = regenRate;

        healthContext.target = gameObject;
        if (hasHealthbar)
        {
            // create healthbar
            healthbar = Instantiate(healthbarPrefab);
            worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform;
            healthbar.transform.SetParent(worldSpaceCanvas, false);
            healthbar.GetComponent<Healthbar>().Initialize(this, gameObject);
        }
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
            HideHealthbar();
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
        ShowHealthBar();
    }

    /// <summary>
    /// Hides the healthbar (for entities that can respawn after dying)
    /// </summary>
    public void HideHealthbar()
    {
        if (hasHealthbar)
        {
            healthbar.SetActive(false);
        }
    }

    public void ShowHealthBar()
    {
        if (hasHealthbar)
        {
            healthbar.SetActive(true);
        }
    }

    /// <summary>
    /// Destroys teh healthbar GameObject (for entities that do not respawn)
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
