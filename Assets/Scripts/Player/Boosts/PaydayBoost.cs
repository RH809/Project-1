/// <summary>
/// This is the scriptable object for the payday boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Payday Boost", menuName = "Scriptable Objects/Boosts/Payday")]
public class PaydayBoost : Boost
{
    [SerializeField] private int payAmount;

    public override void Select()
    {
        Debug.Log("Selected Payday");
        Player.Instance.Bank.AddMoney(payAmount);
    }
}
