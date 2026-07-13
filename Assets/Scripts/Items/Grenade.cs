/// <summary>
/// This script handles the behavior of the grenade.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float detonationTime;
    [SerializeField] private float throwForce;

    [SerializeField] private GameObject explosion;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float damage;
    private float damageMultiplier;
    private Rigidbody rb;

    private HashSet<GameObject> hitSet;

    void Start()
    {
        hitSet = new HashSet<GameObject>();
        rb = GetComponent<Rigidbody>();
        damageMultiplier = (Player.Instance.PowerUp.Active ? Player.Instance.PowerUp.DamageMultiplier : 1f);
        Vector3 throwVector = rb.transform.forward * throwForce;
        rb.AddForce(throwVector, ForceMode.Impulse);
        StartCoroutine(DetonationTimer());
    }

    IEnumerator DetonationTimer()
    {
        yield return new WaitForSeconds(detonationTime);
        Detonate();
    }

    private void Detonate()
    {
        //Debug.Log("Detonating...");
        Instantiate(explosion, transform.position, transform.rotation); // create explosion vfx
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        foreach (Collider hit in hits)
        {
            ZombieBodyPart bodyPart;
            if (hit.gameObject.TryGetComponent<ZombieBodyPart>(out bodyPart))
            {
                //Debug.Log("Greande hit: " + hit.name);
                GameObject zombie = bodyPart.Zombie;
                if (!hitSet.Contains(zombie))
                {
                    hitSet.Add(zombie);
                    zombie.GetComponent<Health>().TakeDamage(damage * damageMultiplier, Player.Instance.gameObject);
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
