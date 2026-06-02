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

    [SerializeField] private GameObject zap;
    [SerializeField] protected Transform shooter;

    protected GameObject targetObject;
    protected DefenderTarget target;
    protected float currentDamage;
    protected float currentCooldown;

    protected override void Update()
    {
        base.Update();
        if (alive)
        {
            cooldown -= Time.deltaTime;
            if (targetObject == null || !targetObject.GetComponent<Health>().IsAlive)
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
        targetObject = DefenderManager.Instance.GetDefenderTarget(transform.position, range);
        target = targetObject.GetComponent<DefenderTarget>();
    }

    protected void Shoot()
    {
        GameObject newZap = Instantiate(zap, shooter.position, Quaternion.identity);
        newZap.GetComponent<DefenderZap>().Initialize(target, currentDamage);
        Debug.Log("Defender shooting...");
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(shooter.position, shooter.position - target.GetDefenderTarget());
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
