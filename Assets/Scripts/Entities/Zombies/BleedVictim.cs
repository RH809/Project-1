/// <summary>
/// This script handles the bleeding behavior for zombies.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class BleedVictim : MonoBehaviour
{
    private Health health;
    private float bleedDamage;
    private int bleedTicks = 0;
    private Queue<KeyValuePair<float, int>> bleedQueue;
    [SerializeField] private float bleedInterval = 0.5f;
    private float countdown;

    void Start()
    {
        bleedQueue = new Queue<KeyValuePair<float, int>>();
    }

    void Update()
    {
        if (bleedTicks == 0)
        {
            if (bleedQueue.Count > 0)
            {
                KeyValuePair<float, int> nextBleed = bleedQueue.Dequeue();
                bleedDamage = nextBleed.Key;
                bleedTicks = nextBleed.Value;
            }
        }
        else
        {
            countdown -= Time.deltaTime;
            if (countdown >= 0)
            {
                health.TakeDamage(bleedDamage / 4, Player.Instance.gameObject);
                bleedTicks--;
                countdown = bleedInterval;
            }
        }
    }

    public void Bleed(float bleedDamage)
    {
        this.bleedDamage = Mathf.Max(bleedDamage, this.bleedDamage);
        bleedQueue.Enqueue(new KeyValuePair<float, int>(bleedDamage, 4 - bleedTicks));
    }
}
