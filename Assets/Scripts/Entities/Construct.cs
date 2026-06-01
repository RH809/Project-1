using UnityEngine;

public abstract class Construct : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    protected Health health;

    protected bool alive = true;
    public bool isAlive { get => alive; }


    protected void Start()
    {
        health = GetComponent<Health>();
    }

    protected void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            if (alive) health.TakeDamage(health.MaxHealth, gameObject);
            else Respawn();
        }
    }

    protected void OnEnable()
    {
        Health.OnDie += Die;
    }

    protected void OnDisable()
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
