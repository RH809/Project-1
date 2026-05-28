using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MiniZombieAttack : ZombieAttack
{
    private int armNum = 0; // 0 = right, 1 = left;
    public override void Attack()
    {
        if (attacking || GetNumAttachedArms() == 0 || cooldown > 0) return;
        attacking = true;
        Debug.Log("Starting mini zombie attack");
        zombieAnimator.SetTrigger("Zombie Attack");
        StartCoroutine(DoAttack());

    }
    public override void ZombieAttackStart()
    {
        Debug.Log("mini zombie attack start");
        collisionHandler.StartAttack();
    }

    public override void ZombieAttackEnd()
    {
        Debug.Log("mini zombie attack end");
        collisionHandler.EndAttack();
    }

    public void StopAttack()
    {
        attacking = false;
    }

    IEnumerator DoAttack()
    {
        while (attacking)
        {
            if (GetNumAttachedArms() == 0)
            {
                attacking = false;
            }
            else if (cooldown <= 0)
            {
                if (armNum == 0 || !leftArm.Attached)
                {
                    Debug.Log("Left attack");
                    zombieAnimator.SetTrigger("Right Attack");
                    zombieAnimator.ResetTrigger("Left Attack");
                }
                else
                {
                    Debug.Log("Right attack");
                    zombieAnimator.SetTrigger("Left Attack");
                    zombieAnimator.ResetTrigger("Right Attack");
                }
                cooldown = attackCooldown;
                armNum = 1 - armNum;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        zombieAnimator.SetTrigger("Zombie Attack End");
        yield return null;
    }
}
