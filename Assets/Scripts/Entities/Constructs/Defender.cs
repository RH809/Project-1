/// <summary>
/// This script is the abstract parent of the two types of defender constructs,
/// which handles their basic behavior.
/// </summary>
using UnityEngine;

public abstract class Defender : Construct
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

    public virtual bool Repairable { get => health.CurrentHealth < health.MaxHealth; }

    protected override void Update()
    {
        base.Update();
        if (alive) // shoots even if not active
        {
            currentCooldown -= Time.deltaTime;
            if (targetObject == null || !targetObject.GetComponent<Health>().IsAlive)
            {
                //Debug.Log("Defender choosing target...");
                ChooseTarget();
                currentDamage = baseDamage; // reset damage
            }
            else if (currentCooldown <= 0)
            {
                //Debug.Log("Defender shooting...");
                Shoot();
                currentDamage += damageIncrement; // increase damage
                currentCooldown = cooldown;
            }
        }
        else
        {
            //Debug.Log("Defender not alive");
        }
    }

    protected void ChooseTarget()
    {
        targetObject = DefenderManager.Instance.GetDefenderTarget(transform.position, range);
        if (targetObject != null)
        {
            target = targetObject.GetComponent<DefenderTarget>();
            if (target == null) // for tank zombie
            {
                target = targetObject.GetComponentInChildren<DefenderTarget>();
            }
        }
    }

    protected void Shoot()
    {
        GameObject newZap = Instantiate(zap, shooter.position, Quaternion.identity);
        newZap.GetComponent<DefenderZap>().Initialize(targetObject, target, currentDamage);
        //Debug.Log("Defender shooting...");
    }

    public void Repair()
    {
        if (!Repairable)
        {
            //Debug.Log("Not repairable");
            return;
        }
        // Respawn or heal by half health
        if (!alive)
        {
            Respawn(0.5f);
        }
        else
        {
            health.Heal(health.MaxHealth / 2);
        }
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
