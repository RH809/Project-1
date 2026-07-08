/// <summary>
/// This script handles the respawn times of the disruptors.
/// </summary>
using UnityEngine;

public class DisruptorManager : Singleton<DisruptorManager>
{
    [SerializeField] private int baseRespawnTime;
    [SerializeField] Disruptor leftDisruptor;
    [SerializeField] Disruptor rightDisruptor;
    public int RespawnTime { get => baseRespawnTime - Player.Instance.Boosts.TechnologicalAdvancement.CooldownDecrease; }

    public void DecreaseRespawnTimes(int decreaseAmount)
    {
        leftDisruptor.DecreaseRespawnTime(decreaseAmount);
        rightDisruptor.DecreaseRespawnTime(decreaseAmount);
    }
    
}
