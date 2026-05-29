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
        Vector3 instantiatePos = transform.position + transform.forward * directionOffset;
        instantiatePos.y = transform.position.y + yOffset;
        Instantiate(tankZombieAttack, instantiatePos, Quaternion.identity);
    }

    public override void ZombieAttackEnd()
    {
        attacking = false;
    }

    void LateUpdate()
    {
        // lock rotation every frame
        moveCollider.transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
