/// <summary>
/// This script handles the attack state of the zombies.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] protected Animator zombieAnimator;
    [SerializeField] protected ZombieArm leftArm;
    [SerializeField] protected ZombieArm rightArm;
    protected ZombieAttackCollider collisionHandler;

    [SerializeField] protected float attackCooldown;
    [SerializeField] private bool disableMovement;

    private static string attackTriggerName = "Zombie Attack";

    protected bool attacking = false;
    public bool IsAttacking { get => attacking; }
    public bool DisableMovement { get => disableMovement; }

    protected float cooldown;

    protected ZombieTarget target;
    public ZombieTarget Target { get => target; }

    protected void Start()
    {
        collisionHandler = GetComponentInChildren<ZombieAttackCollider>(true);
    }

    protected void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Gets the number of attached arms to determine attack damage and whether the zombie can attack
    /// </summary>
    /// <returns>The number of attached arms</returns>
    public int GetNumAttachedArms()
    {
        int num = 0;
        if (leftArm.Attached) num++;
        if (rightArm.Attached) num++;
        return num;
    }

    public virtual bool Attack(ZombieTarget target)
    {
        if (attacking || GetNumAttachedArms() == 0 || cooldown > 0) return false;
        this.target = target;
        attacking = true;
        cooldown = attackCooldown;
        zombieAnimator.SetTrigger(attackTriggerName);
        return true;
    }

    public virtual void ZombieAttackStart()
    {
        //Debug.Log("zombie attack start");
        collisionHandler.StartAttack();
    }

    public virtual void ZombieAttackEnd()
    {
        //Debug.Log("zombie attack end");
        attacking = false;
        collisionHandler.EndAttack();
    }
}
