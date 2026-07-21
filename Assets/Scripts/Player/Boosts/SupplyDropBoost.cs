/// <summary>
/// This is the scriptable object for the supply drop boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Supply Drop Boost", menuName = "Scriptable Objects/Boosts/Supply Drop")]
public class SupplyDropBoost : Boost
{
    [SerializeField] private int numRepairTools;
    [SerializeField] private int numGrenades;
    [SerializeField] private int numHealthPotions;

    public override void Select()
    {
        for (int i = 0; i < numRepairTools; i++)
        {
            Player.Instance.Inventory.AddRepairTool();
        }
        for (int i = 0; i < numGrenades; i++)
        {
            Player.Instance.Inventory.AddGrenade();
        }
        for (int i = 0; i < numHealthPotions; i++)
        {
            Player.Instance.Inventory.AddPotion();
        }
    }
}
