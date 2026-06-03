/// <summary>
/// This script handles the behavior for the inner defender construct.
/// </summary>
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
        active = makeActive;
    }

}
