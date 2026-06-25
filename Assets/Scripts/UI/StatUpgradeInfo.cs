/// <summary>
/// This scriptable object stores the info of a stat upgrade.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Stat Info", menuName = "Scriptable Objects/Stat Upgrade Info")]
public class StatUpgradeInfo : ShopItemInfo
{
    public float statValue;
    public float statValueIncrement;

    public override void Purchase()
    {
        base.Purchase();
        statValue += statValueIncrement;
        description = statValue.ToString() + " => " + (statValue + statValueIncrement).ToString();
    }
}
