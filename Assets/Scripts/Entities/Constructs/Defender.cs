/// <summary>
/// This script handles the behavior for the defender construct.
/// </summary>
using UnityEngine;

public class Defender : Construct
{
    [SerializeField] protected float cooldown;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float damageIncrement;
    [SerializeField] protected float range;

    [SerializeField] protected Transform shooter;

    protected GameObject target;
    protected float currentDamage;
    protected float currentCooldown;

    protected override void Update()
    {
        base.Update();
        if (alive)
        {
            cooldown -= Time.deltaTime;
            if (target == null || !target.GetComponent<Health>().IsAlive)
            {
                ChooseTarget();
                currentDamage = baseDamage; // reset damage
            }
            else if (cooldown <= 0)
            {
                Shoot();
                currentDamage += damageIncrement; // increase damage
                currentCooldown = cooldown;
            }
        }
    }

    protected void ChooseTarget()
    {
        target = DefenderManager.Instance.GetDefenderTarget(transform.position, range);
    }

    protected void Shoot()
    {
        Debug.Log("Defender shooting...");
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(shooter.position, shooter.position - target.transform.position);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
