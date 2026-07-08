/// <summary>
/// This script handles the collision detection of the sword swing.
/// </summary>

using UnityEngine;
using System.Collections.Generic;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private Transform swordTip;
    [SerializeField] private Transform swordBase;

    [SerializeField] private float radius = 0.15f;
    [SerializeField] private float critMultiplier = 1.5f;
    private float damageMultiplier = 1f; // damage multiplier for power up
    private bool isCrit = false;

    private PlayerCamera playerCamera;

    //private Vector3 prevTipPos;
    //private Vector3 prevBasePos;
    private bool inAttackSwing = false;
    [SerializeField] private LayerMask attackMask;

    private HashSet<GameObject> hits;

    Vector3 debugBasePos;
    Vector3 debugTipPos;

    void Start()
    {
        //prevTipPos = swordTip.position;
        //prevBasePos = swordBase.position;
        playerCamera = GetComponent<PlayerCamera>();

        hits = new HashSet<GameObject>();
    }
    public void HitDetection()
    {
        if (!inAttackSwing) return;
        Vector3 currentTipPos = swordTip.position;
        Vector3 currentBasePos = swordBase.position;
        debugBasePos = currentBasePos;
        debugTipPos = currentTipPos;
        /*
        Vector3 prevCenter = (prevBasePos + prevTipPos) * 0.5f;
        Vector3 currCenter = (currentBasePos + currentTipPos) * 0.5f;

        Vector3 move = currCenter - prevCenter;
        if (Physics.CapsuleCast(
            prevBasePos,
            prevTipPos,
            radius,
            move.normalized,
            out RaycastHit hit,
            move.magnitude,
            attackMask))
        {

            Debug.Log("Hit " + hit.collider.name);
        }*/

        Collider[] overlaps = Physics.OverlapCapsule(
            currentBasePos,
            currentTipPos,
            radius,
            attackMask
        );

        foreach (var c in overlaps)
        {
            //Debug.Log("Sword collision: " + c);
            // Check if collider is a zombie body part
            ZombieBodyPart bodyPart = c.gameObject.GetComponent<ZombieBodyPart>();
            if (bodyPart != null && !hits.Contains(bodyPart.Zombie))
            {

                hits.Add(bodyPart.Zombie); // add the zombie to hit list so that it is not hit again in the same swing
                bodyPart.TakeDamage(Shop.Instance.swordDamage.statValue * (isCrit ? critMultiplier : 1f) * damageMultiplier, Player.Instance.gameObject);
                if (Player.Instance.Boosts.hemorrhageBoost.IsActive)
                {
                    bodyPart.Zombie.GetComponent<BleedVictim>().Bleed(Player.Instance.Boosts.hemorrhageBoost.BleedDamage * damageMultiplier);
                }
                //Debug.Log("Hit: " + c.name);
            }
        }

        /*
        prevTipPos = currentTipPos;
        prevBasePos = currentBasePos;
        */
    }

    public void SwordAttackStart() {
        //prevTipPos = swordTip.position;
        //prevBasePos = swordBase.position;
        inAttackSwing = true;
        hits.Clear(); // reset hit list
        float rand = Random.Range(0.0f, 0.9999f);
        isCrit = rand < Shop.Instance.swordCritChance.statValue;
        damageMultiplier = (Player.Instance.PowerUp.Active ? Player.Instance.PowerUp.DamageMultiplier : 1f);
        //if (isCrit) Debug.Log("Sword Attack will crit");
    }

    public void SwordAttackEnd()
    {
        inAttackSwing = false;
        playerCamera.UnlockCamera();
    }

    void OnDrawGizmos()
    {
        if (!inAttackSwing) return;
        Gizmos.color = (inAttackSwing ? Color.green : Color.red);

        // Previous capsule
        DrawCapsule(debugBasePos, debugTipPos, radius);
    }

    void DrawCapsule(Vector3 a, Vector3 b, float r)
    {
        Gizmos.DrawLine(a, b);
        Gizmos.DrawWireSphere(a, r);
        Gizmos.DrawWireSphere(b, r);
    }

    public bool isSwinging() {
        return inAttackSwing;
    }
}
