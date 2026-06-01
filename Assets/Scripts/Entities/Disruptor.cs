using System.Collections;
using UnityEngine;

/// <summary>
/// This script handles the dying and respawning of the disruptor.
/// </summary>
/// <summary>
/// This script handles the behavior for the disruptor construct.
/// </summary>
public class Disruptor : Construct
{
    [SerializeField] private float respawnTime;
    protected override void Die(HealthContext healthContext)
    {
        base.Die(healthContext);
        StartCoroutine(RespawnTime());
    }

    IEnumerator RespawnTime() {
        yield return new WaitForSeconds(respawnTime);
        if (!alive) Respawn();
    }

}
