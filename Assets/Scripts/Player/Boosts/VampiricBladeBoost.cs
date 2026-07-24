/// <summary>
/// This is the scriptable object for the lifesteal boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Vampiric Blade Boost", menuName = "Scriptable Objects/Boosts/Vampiric Blade")]
public class VampiricBladeBoost : Boost
{
    [SerializeField] private float lifestealIncrement;
    private float lifestealPercentage = 0f;
    public float LifestealPercentage { get => lifestealPercentage; }

    public override string Description { get => (level == 0 ? boostDescription :
            $"Increase the healing percentage from {(lifestealPercentage * 100)}% to {((lifestealPercentage + lifestealIncrement) * 100)}%."); }

    public override void Select()
    {
        lifestealPercentage += lifestealIncrement;
        base.Select();
    }
}
