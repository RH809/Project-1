/// <summary>
/// This script handles the attacking behavior of the tank zombie variant.
/// </summary>
using UnityEngine;

public class TankZombieAttack : ZombieAttack
{
    [SerializeField] private GameObject tankZombieAttack;
    [SerializeField] private float directionOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private Collider moveCollider;

    /// <summary>
    /// Instantiate the attack GameObject when the animation reaches the attack point
    /// </summary>
    public override void ZombieAttackStart()
    {
        //Debug.Log("Tank zombie attack start");
        Vector3 instantiatePos = transform.position + transform.forward * directionOffset;
        instantiatePos.y = transform.position.y + yOffset;
        GameObject attackCollider = Instantiate(tankZombieAttack, instantiatePos, Quaternion.identity);
        attackCollider.GetComponent<TankZombieAttackCollider>().SetNumArms(GetNumAttachedArms());
    }

    public override void ZombieAttackEnd()
    {
        attacking = false;
        //Debug.Log($"Tank zombie attack end {IsAttacking}");
    }

    void LateUpdate()
    {
        // lock rotation every frame
        moveCollider.transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
