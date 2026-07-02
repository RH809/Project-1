/// <summary>
/// This script handles the beahvior for the beacon construct.
/// </summary>
using UnityEngine;

public class Beacon : Construct
{
    [SerializeField] private Construct[] innerDefenders;
    [SerializeField] private Construct[] disruptors;

    protected override void Update()
    {
        base.Update();
        bool makeActive = false;
        foreach (Construct d in disruptors)
        {
            if (!d.IsAlive)
            {
                makeActive = true; // maybe can be active if at least one disruptor is dead
            }
        }
        foreach (Construct d in innerDefenders)
        {
            if (d.IsAlive)
            {
                makeActive = false; // if either inner defender is alive, don't make active
            }
        }
        if (!active && makeActive)
        {
            Activate();
        }
        else if (active && !makeActive)
        {
            Deactivate();
        }

        if (Input.GetKeyDown("u"))
        {
            if (alive) health.TakeDamage(health.MaxHealth, gameObject);
        }
    }

    protected override void Respawn(float maxHealthProportion)
    {
        return; // beacon cannot respawn
    }
}
