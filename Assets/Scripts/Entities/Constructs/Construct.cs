/// <summary>
/// This script is the abstract parent for all constructs, which handles the basic dying/respawning logic.
/// </summary>
using UnityEngine;

public abstract class Construct : MonoBehaviour
{
    public enum Type {
        DEFENDER,
        DISRUPTOR,
        BEACON
    };

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
            health.SetImmune(true);
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
            animator.ResetTrigger("Respawn");
            alive = false;
            GameManager.Instance.AddAnnouncement(deathAnnouncement);
        }
    }

    protected virtual void Respawn(float maxHealthProportion)
    {
        animator.SetTrigger("Respawn");
        animator.ResetTrigger("Die");
        health.Respawn(maxHealthProportion, active);
        alive = true;
        GameManager.Instance.AddAnnouncement(respawnAnnouncement);
    }

    protected virtual void Activate()
    {
        active = true;
        health.SetImmune(false);
        if (alive)
        {
            health.ShowHealthBar();
        } 
    }

    protected virtual void Deactivate()
    {
        active = false;
        health.SetImmune(true);
        health.HideHealthbar();
    }

    public abstract Type GetConstructType();
}
