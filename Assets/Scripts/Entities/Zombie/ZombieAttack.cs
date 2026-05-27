/// <summary>
/// This script handles the attack state of the zombies.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private Animator zombieAnimator;
    [SerializeField] private ZombieArm leftArm;
    [SerializeField] private ZombieArm rigthArm;
    private Zombie zombie;
    private ZombieAttackCollider collisionHandler;

    [SerializeField] private float attackCooldown;
    [SerializeField] private bool disableMovement;

    private static string attackTriggerName = "Zombie Attack";

    private bool attacking = false;
    public bool IsAttacking { get => attacking; }

    private float cooldown;

    void Start()
    {
        zombie = GetComponent<Zombie>();
        collisionHandler = GetComponentInChildren<ZombieAttackCollider>();
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Attack();
        }
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
        if (rigthArm.Attached) num++;
        return num;
    }

    public void Attack()
    {
        if (attacking || GetNumAttachedArms() == 0 || cooldown > 0) return;
        attacking = true;
        cooldown = attackCooldown;
        zombieAnimator.SetTrigger(attackTriggerName);
    }

    public void ZombieAttackStart()
    {
        //Debug.Log("zombie attack start");
        collisionHandler.StartAttack();
    }

    public void ZombieAttackEnd()
    {
        //Debug.Log("zombie attack end");
        attacking = false;
        collisionHandler.EndAttack();
    }
}
