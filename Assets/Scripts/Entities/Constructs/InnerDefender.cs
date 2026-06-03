/// <summary>
/// This script handles the behavior for the inner defender construct.
/// </summary>
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InnerDefender : Defender
{
    [SerializeField] private Disruptor[] disruptors;

    protected override void Update()
    {
        base.Update();
        bool makeActive = false;
        foreach (Disruptor d in disruptors)
        {
            if (!d.IsAlive)
            {
                makeActive = true;
                break;
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

        if (Input.GetKeyDown("i"))
        {
            if (alive) health.TakeDamage(health.MaxHealth, gameObject);
            else Repair();
        }
    }

}
