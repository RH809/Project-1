/// <summary>
/// This scriptable object stores the info of a stat upgrade.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Stat Upgrade", menuName = "Scriptable Objects/Stat Upgrade")]
public class StatUpgrade : ShopItem
{
    public float statValue;
    public float statValueIncrement;
    public bool asPercentage;

    public override void Purchase()
    {
        base.Purchase();
        statValue += statValueIncrement;
        if (asPercentage) description = (statValue * 100).ToString("0.##") + "% => " + ((statValue + statValueIncrement) * 100).ToString("0.##") + "%";
        else description = statValue.ToString() + " => " + (statValue + statValueIncrement).ToString();
    }
}
