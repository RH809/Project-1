/// <summary>
/// This is the scriptable object for the technological advancement boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Technological Advancement Boost", menuName = "Scriptable Objects/Boosts/Technological Advancement")]
public class TechnologicalAdvancementBoost : Boost
{
    [SerializeField] private int cooldownDecreaseIncrement = 30;
    private int cooldownDecrease = 0;
    public int CooldownDecrease { get => cooldownDecrease; }
    public override string Description { get => $"Decrease disruptor respawn time by 30 seconds.\n" +
            $"{DisruptorManager.Instance.RespawnTime} => {DisruptorManager.Instance.RespawnTime - cooldownDecreaseIncrement}"; }

    public override void Select()
    {
        cooldownDecrease += cooldownDecreaseIncrement;
        DisruptorManager.Instance.DecreaseRespawnTimes(cooldownDecreaseIncrement);
        level++;
    }
}
