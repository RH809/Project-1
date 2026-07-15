using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// This script handles the dying and respawning of the disruptor.
/// </summary>
/// <summary>
/// This script handles the behavior for the disruptor construct.
/// </summary>
public class Disruptor : Construct
{
    [SerializeField] private Defender defender;
    [SerializeField] private TextMeshProUGUI respawnTimeText;
    private int t;
    private Coroutine respawnRoutine;

    protected override void Start()
    {
        base.Start();
        respawnTimeText.enabled = false;
    }

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
        if (GameManager.Instance.DEBUG && Input.GetKeyDown("o"))
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
            respawnRoutine = StartCoroutine(RespawnTime());
        }
        else if (healthContext.target.Equals(defender.gameObject))
        {

            //Debug.Log("defender died; setting active");
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
        respawnTimeText.enabled = true;
        for (t = DisruptorManager.Instance.RespawnTime; t > 0; t--)
        {
            respawnTimeText.text = timeToText(t);
            yield return new WaitForSeconds(1);
        }
        respawnTimeText.enabled = false;
        if (!alive) Respawn(1);
        respawnRoutine = null;
    }

    public void DecreaseRespawnTime(int decreaseAmount)
    {
        if (respawnRoutine != null)
        {
            t -= decreaseAmount;
        }
    }

    string timeToText(int t)
    {
        return (t / 60).ToString() + ":" + (t % 60).ToString("D2");
    }

    public override Type GetConstructType()
    {
        return Type.DISRUPTOR;
    }

}
