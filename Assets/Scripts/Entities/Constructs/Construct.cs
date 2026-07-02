/// <summary>
/// This script is the abstract parent for all constructs, which handles the basic dying/respawning logic.
/// </summary>
using UnityEngine;

public abstract class Construct : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    protected Health health;

    protected bool alive = true;
    [SerializeField] protected bool active;

    [SerializeField] protected string deathAnnouncement;
    [SerializeField] protected string respawnAnnouncement;
    public bool IsAlive { get => alive; }
    public bool IsActive { get => active;  }


    protected virtual void Start()
    {
        health = GetComponent<Health>();
        if (!active)
        {
            health.HideHealthbar();
        }
    }

    protected virtual void Update()
    {
        /*
        if (Input.GetKeyDown("t"))
        {
            if (alive) health.TakeDamage(health.MaxHealth, gameObject);
            else Respawn(1);
        }
        */
    }

    protected virtual void OnEnable()
    {
        Health.OnDie += Die;
    }

    protected virtual void OnDisable()
    {
        Health.OnDie -= Die;
    }

    protected virtual void Die(HealthContext healthContext)
    {
        if (healthContext.target.Equals(gameObject))
        {
            animator.SetTrigger("Die");
            alive = false;
            GameManager.Instance.AddAnnouncement(deathAnnouncement);
        }
    }

    protected virtual void Respawn(float maxHealthProportion)
    {
        animator.SetTrigger("Respawn");
        health.Respawn(maxHealthProportion, active);
        alive = true;
        GameManager.Instance.AddAnnouncement(respawnAnnouncement);
    }

    protected virtual void Activate()
    {
        active = true;
        if (alive)
        {
            health.ShowHealthBar();
        } 
    }

    protected virtual void Deactivate()
    {
        active = false;
        health.HideHealthbar();
    }
}
