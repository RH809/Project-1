/// <summary>
/// This script handles the behavior for the outer defender construct.
/// </summary>
using UnityEngine;

public class OuterDefender : Defender
{
    [SerializeField] private Disruptor disruptor;

    public override bool Repairable { get => health.CurrentHealth < health.MaxHealth && disruptor.IsAlive; }
}
