/// <summary>
/// This is the scriptable object for the lifesteal boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Vampiric Blade Boost", menuName = "Scriptable Objects/Boosts/Vampiric Blade")]
public class VampiricBladeBoost : Boost
{
    [SerializeField] private float lifestealMultiplier;
    private float lifestealPercentage = 0.01f;
    public float LifestealPercentage { get => lifestealPercentage; }

    public override string Description { get => (level == 0 ? boostDescription :
            $"Increase healing percentage from {(lifestealPercentage * 100)}% to {(lifestealPercentage * lifestealMultiplier * 100)}%."); }

    public override void Select()
    {
        lifestealPercentage *= lifestealMultiplier;
        base.Select();
    }
}
