/// <summary>
/// This is the scriptable object for the collector boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Collector Boost", menuName = "Scriptable Objects/Boosts/Collector")]
public class CollectorBoost : Boost
{
    [SerializeField] private float collectorIncrement = 1.0f / 3.0f;
    private float multiplier = 1.0f;
    public float Multiplier { get => multiplier; }
    public override string Description { get => (level == 0 ? boostDescription :
            $"Increase bonus money drop from {(multiplier * 100):#:##}% to {((multiplier + collectorIncrement) * 100):#:##}%."); }
    public override void Select()
    {
        multiplier += collectorIncrement;
        level++;
    }
}
