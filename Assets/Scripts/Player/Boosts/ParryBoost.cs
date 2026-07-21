/// <summary>
/// This is the scriptable object for the parry boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Parry Boost", menuName = "Scriptable Objects/Boosts/Parry")]
public class ParryBoost : Boost
{
    [SerializeField] private float parryChanceIncrement;
    private float parryChance;
    public float ParryChance { get => parryChance; }
    
    public override string Description { get => (level == 0 ? boostDescription :
            $"Increase the parry chance from {(parryChance * 100):0.##}% to {((parryChance + parryChanceIncrement) * 100):0.##}%."); }

    public override void Select()
    {
        parryChance += parryChanceIncrement;
        base.Select();
    }
}
