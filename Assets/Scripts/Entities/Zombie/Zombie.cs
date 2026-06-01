/// <summary>
/// This script handles the zombie's death and acts as an identifying component.
/// </summary>

using UnityEngine;

public class Zombie : MonoBehaviour
{
    public enum ZombieType
    {
        TANK,
        REGULAR,
        MINI
    }

    [SerializeField] private ZombieType type;
    public ZombieType Type { get => type; }

    private Health health;
    private Vector3 contactPos;
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

    void Die(HealthContext healthCtx)
    {
        if (healthCtx.target == gameObject)
        {
            Debug.Log("Zombie killed");
            health.DestroyHealthbar();
            Destroy(gameObject);
        }
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Zombie collided with " + collision.collider);
        if (collision.collider.GetComponentInParent<Disruptor>() != null) {
            contactPos = collision.GetContact(0).point;
            foreach (var contact in collision.contacts)
            {
                Debug.Log($"THIS collider: {contact.thisCollider.name}");
                Debug.Log($"OTHER collider: {contact.otherCollider.name}");
            }
        }
        
        
    }
    */

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(contactPos, 0.5f);
    }
}
