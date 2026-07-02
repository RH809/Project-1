using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MiniZombieAttack : ZombieAttack
{
    private int armNum = 0; // 0 = right, 1 = left;
    public override bool Attack(ZombieTarget target)
    {
        if (attacking || GetNumAttachedArms() == 0 || cooldown > 0) return false;
        this.target = target;
        attacking = true;
        //Debug.Log("Starting mini zombie attack");
        zombieAnimator.SetTrigger("Zombie Attack");
        StartCoroutine(DoAttack());
        return true;

    }
    public override void ZombieAttackStart()
    {
        //Debug.Log("mini zombie attack start");
        collisionHandler.StartAttack();
    }

    public override void ZombieAttackEnd()
    {
        //Debug.Log("mini zombie attack end");
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
                if (armNum == 0)
                {
                    //Debug.Log("Right attack");
                    if (rightArm.Attached)
                    {
                        zombieAnimator.SetTrigger("Right Attack");
                    }
                    zombieAnimator.ResetTrigger("Left Attack");
                    //yield return new WaitUntil(() => zombieAnimator.GetAnimatorTransitionInfo(1).IsName(
                    //    "Mini Zombie Right Attack -> Mini Zombie Attack Start"));
                }
                else
                {
                    //Debug.Log("Left attack");
                    if (leftArm.Attached)
                    {
                        zombieAnimator.SetTrigger("Left Attack");
                    }
                    zombieAnimator.ResetTrigger("Right Attack");
                    //yield return new WaitUntil(() => zombieAnimator.GetAnimatorTransitionInfo(1).IsName(
                    //    "Mini Zombie Left Attack -> Mini Zombie Attack Start"));
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
