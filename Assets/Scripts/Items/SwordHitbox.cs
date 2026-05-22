using UnityEngine;
using System.Collections.Generic;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private Transform swordTip;
    [SerializeField] private Transform swordBase;

    [SerializeField] private float radius = 0.15f;
    [SerializeField] private int damage;

    private PlayerCamera playerCamera;

    //private Vector3 prevTipPos;
    //private Vector3 prevBasePos;
    private bool inAttackSwing = false;
    [SerializeField] private LayerMask attackMask;

    private HashSet<GameObject> hits;

    void Start()
    {
        //prevTipPos = swordTip.position;
        //prevBasePos = swordBase.position;
        playerCamera = GetComponent<PlayerCamera>();

        hits = new HashSet<GameObject>();
    }

    void LateUpdate()
    {
        if (!inAttackSwing) return;
        Vector3 currentTipPos = swordTip.position;
        Vector3 currentBasePos = swordBase.position;
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
            ZombieBodyPart bodyPart = c.gameObject.GetComponent<ZombieBodyPart>();
            if (bodyPart != null && !hits.Contains(bodyPart.Zombie)) {
                hits.Add(bodyPart.Zombie);
                bodyPart.TakeDamage(damage, gameObject);
                Debug.Log("Hit: " + c.name);
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
        hits.Clear();
    }

    public void SwordAttackEnd() {
        inAttackSwing = false;
        playerCamera.UnlockCamera();
    }

    /*
    void OnDrawGizmos()
    {
        if (!inAttackSwing) return;
        Gizmos.color = Color.red;

        // Previous capsule
        DrawCapsule(prevBasePos, prevTipPos, radius);

        Gizmos.color = Color.green;

        // Current capsule
        DrawCapsule(swordBase.position, swordTip.position, radius);

        // Sweep direction (center movement)
        Vector3 prevCenter = (prevBasePos + prevTipPos) * 0.5f;
        Vector3 currCenter = (swordBase.position + swordTip.position) * 0.5f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(prevCenter, currCenter);
    }

    void DrawCapsule(Vector3 a, Vector3 b, float r)
    {
        Gizmos.DrawLine(a, b);
        Gizmos.DrawWireSphere(a, r);
        Gizmos.DrawWireSphere(b, r);
    }
    */

    public bool isSwinging() {
        return inAttackSwing;
    }
}
