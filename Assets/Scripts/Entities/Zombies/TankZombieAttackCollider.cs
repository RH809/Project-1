/// <summary>
/// This script handles the attack collider for the tank zombie variant.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankZombieAttackCollider : MonoBehaviour
{
    [SerializeField] private Vector3 startScale;
    [SerializeField] private Vector3 endScale;
    [SerializeField] private float duration;
    [SerializeField] private float startOuterRadius;
    [SerializeField] private float startInnerRadius;
    [SerializeField] private float height;
    private float damage;
    [SerializeField] private float easyDamage;
    [SerializeField] private float mediumDamage;
    [SerializeField] private float hardDamage;

    private float outerRadius;
    private float innerRadius;
    private int numArms;
    private HashSet<GameObject> hits;

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
        StartCoroutine(Grow());
    }

    public void SetNumArms(int numArms)
    {
        this.numArms = numArms;
    }

    /// <summary>
    /// Grows the attack collider
    /// </summary>
    IEnumerator Grow()
    {
        transform.localScale = startScale;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            transform.localScale = Vector3.Lerp(startScale, endScale, lerp);
            // Calculate current radii
            outerRadius = startOuterRadius * transform.localScale.x / startScale.x;
            innerRadius = startInnerRadius * transform.localScale.x / startScale.x;

            yield return null;
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Handles collisions with zombie targets
    /// </summary>
    /// <param name="other">Collider it collided with</param>
    void OnTriggerEnter(Collider other)
    {
        if (numArms == 0) return;
        GameObject hit = other.gameObject;
        ZombieTarget hitTarget = hit.GetComponent<ZombieTarget>();
        
        if (hits.Contains(hit) || hitTarget == null) return;
        if (hitTarget.GetHitboxBottom().y > height + GameManager.GroundY) return; // didn't hit in the cylinder part
        
        float distToCenter = Mathf.Sqrt(Mathf.Pow(hit.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(hit.transform.position.z - transform.position.z, 2));
        //Debug.Log($"Trigger entered {hit} {hitTarget} {distToCenter} {innerRadius} {outerRadius}");
        if ((distToCenter >= innerRadius && distToCenter <= outerRadius) || // already in between
            (distToCenter < innerRadius && distToCenter + hitTarget.Radius >= innerRadius) || // check inner edge
            (distToCenter > outerRadius && distToCenter - hitTarget.Radius <= outerRadius)) // check outer edge
        {
            //Debug.Log("(Enter) Hit: " + hit + " " + hitTarget.GetHitboxBottom().y);
            hits.Add(hit);
            if (hit == Player.Instance.gameObject && Player.Instance.Inventory.CanParry)
            {   // check for parry
                float rand = Random.Range(0.0f, 1.0f);
                if (rand <= Player.Instance.Boosts.Parry.ParryChance)
                {
                    Debug.Log("Parried!");
                    return;
                }
            }
            hit.GetComponent<Health>().TakeDamage(damage * numArms, gameObject);
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (numArms == 0) return;
        GameObject hit = other.gameObject;
        ZombieTarget hitTarget = hit.GetComponent<ZombieTarget>();
        if (hitTarget == null)
        {
            hitTarget = hit.GetComponentInParent<ZombieTarget>();
            if (hitTarget == null) return;
            hit = hitTarget.gameObject;
        }

        if (hits.Contains(hit)) return;
        if (hitTarget.GetHitboxBottom().y > height + GameManager.GroundY) return; // did't hit in the cylinder part

        float distToCenter = Mathf.Sqrt(Mathf.Pow(hit.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(hit.transform.position.z - transform.position.z, 2));
        //Debug.Log($"Trigger entered {hit} {hitTarget} {distToCenter} {innerRadius} {outerRadius}");
        if ((distToCenter >= innerRadius && distToCenter <= outerRadius) || // already in between
            (distToCenter < innerRadius && distToCenter + hitTarget.Radius >= innerRadius) || // check inner edge
            (distToCenter > outerRadius && distToCenter - hitTarget.Radius <= outerRadius)) // check outer edge
        {
            //Debug.Log("(Stay) Hit: " + hit + " " + hitTarget.GetHitboxBottom().y);
            hits.Add(hit);
            if (hit == Player.Instance.gameObject && Player.Instance.Inventory.CanParry)
            {   // check for parry
                float rand = Random.Range(0.0f, 1.0f);
                if (rand <= Player.Instance.Boosts.Parry.ParryChance)
                {
                    Debug.Log("Parried!");
                    return;
                }
            }
            hit.GetComponent<Health>().TakeDamage(damage * numArms, gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(1, height, 1));
    }
}
