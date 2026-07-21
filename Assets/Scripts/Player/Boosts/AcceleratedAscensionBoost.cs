/// <summary>
/// This is the scriptable object for the accelerated ascension boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Accelerated Ascension Boost", menuName = "Scriptable Objects/Boosts/Accelerated Ascension")]
public class AcceleratedAscensionBoost : Boost
{
    [SerializeField] private int cooldownDecreaseIncrement = 30;
    private int cooldownDecrease = 0;
    public int CooldownDecrease { get => cooldownDecrease; }
    public override string Description { get => $"Decrease the power up spawn time by 30 seconds.\n" +
            $"{PowerUpManager.Instance.SpawnTime} => {PowerUpManager.Instance.SpawnTime - cooldownDecreaseIncrement}"; }

    public override void Select()
    {
        //Debug.Log("Selected Accelerated Ascension");
        cooldownDecrease += cooldownDecreaseIncrement;
        PowerUpManager.Instance.DecreaseSpawnTime(cooldownDecreaseIncrement);
        base.Select();
    }
}
