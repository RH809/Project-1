using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackCollider : MonoBehaviour
{
    [SerializeField] private float easyDamage;
    [SerializeField] private float mediumDamage;
    [SerializeField] private float hardDamage;
    private float damage;
    private HashSet<GameObject> hits;
    private ZombieAttack attack;

    void Start()
    {
        hits = new HashSet<GameObject>();
        switch (SettingsManager.Instance.GameDifficulty)
        {
            case SettingsManager.Difficulty.EASY:
                damage = easyDamage;
                break;
            case SettingsManager.Difficulty.MEDIUM:
                damage = mediumDamage;
                break;
            case SettingsManager.Difficulty.HARD:
                damage = hardDamage;
                break;
        }
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
        ZombieTarget hitTarget = hit.GetComponent<ZombieTarget>();
        if (hitTarget == null)
        {
            hitTarget = hit.GetComponentInParent<ZombieTarget>(); // get from parent as well in the case of defenders
            if (hitTarget == null) return;
            hit = hitTarget.gameObject;
        }
        if (!hit.Equals(attack.Target.gameObject)) return; // only hit target
        //Debug.Log("Hit: " + hit);
        if (!hits.Contains(hit))
        {
            hits.Add(hit);
            hit.GetComponent<Health>().TakeDamage(damage * attack.GetNumAttachedArms(), gameObject);
        }
    }
}
