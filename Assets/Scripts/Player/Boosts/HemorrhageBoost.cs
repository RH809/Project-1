/// <summary>
/// This is the scriptable object for the hemorrhage boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Hemorrhage Boost", menuName = "Scriptable Objects/Boosts/Hemorrhage")]
public class HemorrhageBoost : Boost
{
    [SerializeField] private float bleedDamageIncrement;
    private float bleedDamage = 0;
    public float BleedDamage { get => bleedDamage; }
    public override string Description { get => (level == 0 ? boostDescription :
            $"Increase bleed damage from {bleedDamage} to {bleedDamage + bleedDamageIncrement} over time."); }
    public override void Select()
    {
        //Debug.Log("Selected Hemorrhage");
        bleedDamage += bleedDamageIncrement;
        level++;
    }
}
