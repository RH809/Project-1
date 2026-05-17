using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private Transform swordTip;
    [SerializeField] private Transform swordBase;

    [SerializeField] private float radius = 0.15f;

    private Vector3 prevTipPos;
    private Vector3 prevBasePos;
    private bool inAttackSwing = false;
    private int attackMask;
    void Start()
    {
        prevTipPos = swordTip.position;
        prevBasePos = swordBase.position;

        attackMask = LayerMask.GetMask("Enemy");
    }

    void LateUpdate()
    {
        if (!inAttackSwing) return;
        Vector3 currentTipPos = swordTip.position;
        Vector3 currentBasePos = swordBase.position;

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
        }

        Collider[] overlaps = Physics.OverlapCapsule(
            currentBasePos,
            currentTipPos,
            radius,
            attackMask
        );

        foreach (var c in overlaps)
        {
            Debug.Log("OVERLAP HIT: " + c.name);
        }

        prevTipPos = currentTipPos;
        prevBasePos = currentBasePos;
    }

    public void SwordAttackStart() {
        prevTipPos = swordTip.position;
        prevBasePos = swordBase.position;
        Debug.Log("Starting sword attack");
        inAttackSwing = true;
    }

    public void SwordAttackEnd() {
        Debug.Log("Sword attack end");
        inAttackSwing = false;
    }

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
}
