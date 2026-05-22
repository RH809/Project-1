using UnityEngine;

public class ZombieBodyPart : MonoBehaviour
{
    
    [SerializeField] private int damageMultiplier = 1;
    private GameObject zombie;
    private ZombieArm arm;
    private Health health;

    public GameObject Zombie { get => zombie; }

    void Start()
    {
        zombie = GetComponentInParent<Zombie>().gameObject;
        health = zombie.GetComponent<Health>();
        arm = GetComponentInParent<ZombieArm>();
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        Debug.Log(gameObject + " taking damage");
        health.TakeDamage(damage * damageMultiplier, attacker);
        if (arm != null)
        {
            arm.TakeDamage(damage);
        }
    }
}
