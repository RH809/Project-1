using UnityEngine;

public class Disruptor : MonoBehaviour
{
    [SerializeField] private Animator disruptorAnimator;

    private Health health;

    private bool alive = true;
    public bool isAlive { get => alive; }


    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            if (alive) health.TakeDamage(100, gameObject);
            else Respawn();
        }
    }

    void OnEnable()
    {
        Health.OnDie += Die;
    }

    void OnDisable()
    {
        Health.OnDie -= Die;
    }

    void Die(HealthContext healthContext)
    {
        if (healthContext.target == gameObject)
        {
            disruptorAnimator.SetTrigger("Die");
            alive = false;
        }
    }

    void Respawn()
    {
        disruptorAnimator.SetTrigger("Respawn");
        health.Respawn();
        alive = true;
    }
}
