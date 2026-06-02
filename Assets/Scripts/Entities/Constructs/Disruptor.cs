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
    [SerializeField] private Defender defender;

    protected override void OnEnable()
    {
        base.OnEnable();
        Health.OnRespawn += DefenderRespawn;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Health.OnRespawn -= DefenderRespawn;
    }
    protected override void Die(HealthContext healthContext)
    {
        if (healthContext.source.Equals(gameObject))
        {
            base.Die(healthContext);
            StartCoroutine(RespawnTime());
        }
        else if (healthContext.source.Equals(defender))
        {
            active = true;
        }
    }

    void DefenderRespawn(HealthContext healthContext)
    {
        if (healthContext.target.Equals(defender))
        {
            active = false; // make it not active when defender respawns
        }
    }

    IEnumerator RespawnTime() {
        yield return new WaitForSeconds(respawnTime);
        if (!alive) Respawn();
    }

}
