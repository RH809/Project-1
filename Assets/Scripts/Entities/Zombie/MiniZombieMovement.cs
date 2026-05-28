using UnityEngine;

public class MiniZombieMovement : ZombieMovement
{
    private Health targetHealth;
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
            }
            if (GetDistance(target) <= attackRange + zombieTarget.Radius &&
                    (target != player || (target == player && Player.Instance.Health.IsAlive)))
            {
                attack.Attack();
                if (attack.DisableMovement)
                {
                    moveEnabled = false; // disable movement when beginning attack if applicable
                }
            }
        }
        else
        {
            if (!targetHealth.IsAlive || GetDistance(target) > attackRange + zombieTarget.Radius)
            {
                // Stop attack
                Debug.Log("Stopping mini zombie attack");
                ((MiniZombieAttack)attack).StopAttack();
            }
        }
        return changedTarget;
    }
}
