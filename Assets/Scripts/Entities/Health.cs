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
        regenTimer -= Time.deltaTime;
        if (currentHealth > 0 && regenTimer <= 0) {
            regenTimer = regenRate;
            currentHealth = Mathf.Min(currentHealth + baseRegen, maxHealth);
        }
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        healthContext.source = gameObject;
        OnHeal?.Invoke(healthContext);
    }

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

    public void Respawn()
    {
        currentHealth = maxHealth;
        regenTimer = regenRate;
    }   
    
}

public struct HealthContext
{
    public GameObject source;
    public GameObject target;
}
