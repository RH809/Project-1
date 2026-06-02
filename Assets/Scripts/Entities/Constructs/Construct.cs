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
    public bool IsAlive { get => alive; }
    public bool IsActive { get => active;  }


    protected virtual void Start()
    {
        health = GetComponent<Health>();
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            if (alive) health.TakeDamage(health.MaxHealth, gameObject);
            else Respawn();
        }
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
        if (healthContext.target == gameObject)
        {
            animator.SetTrigger("Die");
            alive = false;
        }
    }

    protected virtual void Respawn()
    {
        animator.SetTrigger("Respawn");
        health.Respawn();
        alive = true;
    }
}
