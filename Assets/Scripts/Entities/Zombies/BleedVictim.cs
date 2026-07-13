/// <summary>
/// This script handles the bleeding behavior for zombies.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class BleedVictim : MonoBehaviour
{
    private Health health;
    private List<BleedInfo> bleeds;
    [SerializeField] private float bleedInterval = 0.5f;
    private float countdown;

    void Start()
    {
        bleeds = new List<BleedInfo>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (countdown <= 0)
        {
            if (bleeds.Count > 0)
            {
                float bleedDamage = 0;
                for (int i = bleeds.Count - 1; i >= 0; i--)
                {
                    bleedDamage += bleeds[i].damage;
                    bleeds[i].ticksRemaining--;
                    if (bleeds[i].ticksRemaining <= 0)
                    {
                        bleeds.RemoveAt(i);
                    }
                }
                health.TakeDamage(bleedDamage / 4.0f, Player.Instance.gameObject);
                if (Player.Instance.Boosts.VampiricBlade.IsActive)
                {
                    Player.Instance.Health.Heal(bleedDamage * Player.Instance.Boosts.VampiricBlade.LifestealPercentage);
                }
                countdown = bleedInterval;
            }
        }
        else
        {
            countdown -= Time.deltaTime;
        }
    }

    public void Bleed(float bleedDamage)
    {
        bleeds.Add(new BleedInfo(bleedDamage));
        if (bleeds.Count == 1) countdown = bleedInterval;
    }
}
class BleedInfo
{
    public float damage;
    public int ticksRemaining;

    public BleedInfo(float damage)
    {
        this.damage = damage;
        this.ticksRemaining = 4;
    }
}
