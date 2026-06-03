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

    protected override void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            if (alive) health.TakeDamage(health.MaxHealth, gameObject);
            else Respawn(1f);
        }
    }

    protected override void Die(HealthContext healthContext)
    {
        if (healthContext.target.Equals(gameObject))
        {
            base.Die(healthContext);
            StartCoroutine(RespawnTime());
        }
        else if (healthContext.target.Equals(defender.gameObject))
        {

            Debug.Log("defender died; setting active");
            Activate();
        }
    }

    void DefenderRespawn(HealthContext healthContext)
    {
        if (healthContext.target.Equals(defender.gameObject))
        {
            Deactivate(); // deactivate when defender respawns
        }
    }

    IEnumerator RespawnTime() {
        yield return new WaitForSeconds(respawnTime);
        if (!alive) Respawn(1);
    }

}
