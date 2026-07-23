/// <summary>
/// This is the scriptable object for the piercing boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Piercing Boost", menuName = "Scriptable Objects/Boosts/Piercing")]
public class PiercingBoost : Boost
{
    [SerializeField] private int piercingMultiplier;
    private int pierceAmount = 1;
    public int PierceAmount { get => pierceAmount; }

    public override string Description { get => (level == 0 ? boostDescription :
        $"Increase the number of zombies the bullets can hit from {pierceAmount} to {pierceAmount * piercingMultiplier}."); }

    public override void Select()
    {
        //Debug.Log("Selected Piercing");
        pierceAmount *= piercingMultiplier;
        base.Select();
    }

}
