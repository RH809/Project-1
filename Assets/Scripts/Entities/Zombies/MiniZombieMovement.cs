using UnityEngine;

public class MiniZombieMovement : ZombieMovement
{
    private Health targetHealth;
    private Construct targetConstruct;
    protected override bool HandleAttack()
    {
        bool changedTarget = false;
        if (!attack.IsAttacking)
        {
            moveEnabled = true; // not attacking currently, so move is allowed
            // Choose target and attack if in range
            changedTarget = ChooseTarget();
            if (changedTarget)
            {
                targetHealth = target.GetComponent<Health>();
                zombieTarget = target.GetComponent<ZombieTarget>();
                targetConstruct = target.GetComponent<Construct>();
            }
            if (GetDistance(target) <= attackRange + zombieTarget.Radius &&
                    (target != player || (target == player && Player.Instance.Health.IsAlive)))
            {
                bool success = attack.Attack(zombieTarget);
                if (success && attack.DisableMovement)
                {
                    moveEnabled = false; // disable movement when beginning attack if applicable
                }
            }
        }
        else
        {
            if (!targetHealth.IsAlive || (targetConstruct != null && !targetConstruct.IsActive) || GetDistance(target) > attackRange + zombieTarget.Radius)
            {
                // Stop attack
                //Debug.Log("Stopping mini zombie attack");
                ((MiniZombieAttack)attack).StopAttack();
            }
        }
        return changedTarget;
    }
}
