using UnityEngine;

public class Zombie : MonoBehaviour
{
    private Health health;
    void Start()
    {
        health = GetComponent<Health>();
    }

    void OnEnable()
    {
        Health.OnDie += Die;
    }

    void OnDisable()
    {
        Health.OnDie -= Die;
    }

    void Die(HealthContext health)
    {
        if (health.target == gameObject)
        {
            Debug.Log("Zombie killed");
            Destroy(gameObject);
        }
    }
}
