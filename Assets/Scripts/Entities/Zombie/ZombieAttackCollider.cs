using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackCollider : MonoBehaviour
{
    [SerializeField] private float damage;
    private HashSet<GameObject> hits;
    private ZombieAttack attack;

    void Start()
    {
        hits = new HashSet<GameObject>();
        attack = GetComponentInParent<ZombieAttack>();
        gameObject.SetActive(false);
    }
    public void StartAttack()
    {
        gameObject.SetActive(true);
        hits.Clear();
    }

    public void EndAttack()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        Debug.Log("Hit: " + hit);
        if (!hits.Contains(hit))
        {
            hits.Add(hit);
            hit.GetComponent<Health>().TakeDamage(damage * attack.GetNumAttachedArms(), this.gameObject);
        }
    }

    /*
    void OnTriggerStay(Collider other)
    {
        GameObject hit = other.gameObject;
        Debug.Log("Hit: " + hit);
        if (!hits.Contains(hit))
        {
            hits.Add(hit);
            hit.GetComponent<Health>().TakeDamage(damage * attack.GetNumAttachedArms(), this.gameObject);
        }
    }
    */
}
