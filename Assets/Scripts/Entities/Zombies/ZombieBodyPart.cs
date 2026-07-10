/// <summary>
/// This script handles the hitbox collisions for the zombies' body parts.
/// </summary>

using UnityEngine;

public class ZombieBodyPart : MonoBehaviour
{
    
    [SerializeField] private int damageMultiplier = 1; // headshots deal double damage
    private GameObject zombie;
    private ZombieArm arm;
    private Health health;

    public GameObject Zombie { get => zombie; }

    void Start()
    {
        zombie = GetComponentInParent<Zombie>().gameObject;
        health = zombie.GetComponent<Health>();
        // Get arm component of self or ancestor in hierachy if it exists
        arm = GetComponent<ZombieArm>();
        if (arm == null) arm = GetComponentInParent<ZombieArm>();
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        //Debug.Log(gameObject + " taking damage");
        health.TakeDamage(damage * damageMultiplier, attacker);
        if (arm != null)
        {
            // Make arm take damage as well if applicable
            arm.TakeDamage(damage);
        }
    }
}
