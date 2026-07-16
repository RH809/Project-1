/// <summary>
/// This script controls the pathfinding of a zombie.
/// </summary>

using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private Animator zombieAnimator;
    private NavMeshAgent agent;

    [SerializeField] private float moveSpeed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] private float playerStoppingDist;
    [SerializeField] private float disruptorStoppingDist;
    [SerializeField] private float defenderStoppingDist;
    [SerializeField] private float beaconStoppingDist;
    [SerializeField] private float maxKiteRange; // kite range will scale inversely with distance from closest constructs
    [SerializeField] private float kiteRangeThreshold; // construct target must be outside of this range for player to take aggro
    [SerializeField] private float playerPriorityRange; // will not prioritize player if it is out of this range

    [SerializeField] private Transform head;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    private Quaternion headBaseRotation;

    protected ZombieAttack attack;
    protected Rigidbody rb;
    protected StunVictim stunVictim;
    
    protected GameObject player;
    protected Construct[] constructTargets; // Given by spawner
    protected Collider constructCollider;

    protected GameObject target;
    protected ZombieTarget zombieTarget;

    protected bool initialized = false; 
    protected bool moveEnabled = true;
    protected bool rbRotation = true;
    private bool wasMoving = false;
    private bool wasStunned = false;

    protected Vector3 prevTargetPos; // for maintaining same rotation when stunned
    private Vector3 lastDestination; // for preventing unnecessary set destinations

    private float updateInterval = 0.25f;
    private float nextUpdateTime;
    private float rotateInterval = 0.06f;
    private float nextRotateTime;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        attack = GetComponent<ZombieAttack>();
        agent = GetComponent<NavMeshAgent>();
        stunVictim = GetComponent<StunVictim>();
        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.speed = moveSpeed;
        player = Player.Instance.gameObject;

        headBaseRotation = head.localRotation;
        nextUpdateTime = Time.fixedTime + Random.Range(0, updateInterval);
        nextRotateTime = Time.fixedTime + Random.Range(0, rotateInterval);
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (!initialized) return;
        if (stunVictim.Stunned)
        {
            if (!wasStunned)
            {
                agent.ResetPath();
                wasStunned = true;
                wasMoving = false;
            } 
            return;
        }
        if (Time.fixedTime < nextUpdateTime && !wasStunned)
        { // stagger updates to reduce lag, but still rotate so that it isn't clunky
            if (!rbRotation) return;
            if (target == player || Time.fixedTime >= nextRotateTime)
            {
                RotateTowardTarget();
                nextRotateTime = Time.fixedTime + rotateInterval;
            }
            return;
        }
        else
        {
            nextUpdateTime = Time.fixedTime + updateInterval;
        }
        bool changedTarget = HandleAttack();
        //Debug.Log($"{moveEnabled} {attack.IsAttacking}");
        if (moveEnabled)
        {
            
            bool stopped = agent.velocity.sqrMagnitude < 0.0001f;
            //Debug.Log($"{target} {wasMoving} {agent.isStopped} {stopped} {changedTarget}");
            if (changedTarget)
            {
                Vector3 destination;
                if (constructCollider != null)
                {
                    destination = constructCollider.ClosestPoint(rb.position);
                }
                else
                {
                    destination = target.transform.position;
                }
                destination.y = rb.position.y;
                agent.SetDestination(destination);
                lastDestination = destination;
                //Debug.Log("Starting movement toward " + destination + " | " + agent.destination);
                zombieAnimator.ResetTrigger("Stop Moving");
                zombieAnimator.SetTrigger("Start Moving");
                wasMoving = true;
            }
            else
            {
                if (target == player)
                {
                    Vector3 destination = player.transform.position;
                    destination.y = rb.position.y;
                    //Debug.Log("Updating player tracking");
                    //Debug.Log("Updating player tracking " + destination + " | " + agent.destination);
                    if ((destination - lastDestination).sqrMagnitude > 0.1f || wasStunned)
                    {
                        agent.SetDestination(destination);
                        lastDestination = destination;
                    }

                }
                else if (constructCollider != null)
                {
                    Vector3 destination = constructCollider.ClosestPoint(rb.position);
                    destination.y = rb.position.y;
                    if ((destination - lastDestination).sqrMagnitude > 0.1f || wasStunned)
                    {
                        agent.SetDestination(destination);
                        lastDestination = destination;
                    }
                }
                if (wasMoving)
                {
                    if (stopped)
                    {
                        //Debug.Log("Stopping movement animation");
                        zombieAnimator.ResetTrigger("Start Moving");
                        zombieAnimator.SetTrigger("Stop Moving");
                        wasMoving = false;
                    }
                    else
                    {
                        //Debug.Log("Still moving");
                        wasMoving = true;
                    }
                }
                else
                {
                    if (!stopped)
                    {
                        //Debug.Log("Starting movement from stop");
                        zombieAnimator.ResetTrigger("Stop Moving");
                        zombieAnimator.SetTrigger("Start Moving");
                        wasMoving = true;
                    }
                    else
                    {
                        //Debug.Log("Still not moving");
                        wasMoving = false;
                    }
                }
            }
            
        }
        else
        {
            if (wasMoving)
            {
                // Stop movement entirely
                //Debug.Log("Stopping movement");
                agent.ResetPath();
                target = null;
                zombieAnimator.ResetTrigger("Start Moving");
                zombieAnimator.SetTrigger("Stop Moving");
            }
            wasMoving = false;
        }
        wasStunned = false;
        // rotate to face target even if not moving
        //Debug.Log($"Target: {target}  {target.transform.position}  {agent.destination}");
        if (!rbRotation) return;
        RotateTowardTarget();
        Debug.DrawRay(rb.position, agent.destination - rb.position, Color.red);
        Debug.DrawRay(rb.position, target.transform.position - rb.position, Color.orange);

        
    }

    void RotateTowardTarget()
    {
        if (target == null) return;
        Vector3 dir = target.transform.position - rb.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.25f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRot,
                    rotationSpeed * Time.fixedDeltaTime
                )
            );
            //Debug.Log("Rotating rigidbody " + rb.rotation);
        }
    }

    protected virtual void LateUpdate()
    {
        // rotate arms and head to point toward target
        if (zombieTarget)
        {
            Vector3 targetPoint = (stunVictim.Stunned ? prevTargetPos : zombieTarget.GetZombieTarget());
            prevTargetPos = targetPoint;
            Vector3 direction = targetPoint - head.position;
            Debug.DrawRay(head.position, direction);
            float targetPitch = -Mathf.Atan2(
                direction.y,
                new Vector2(direction.x, direction.z).magnitude
            ) * Mathf.Rad2Deg;
            float currentPitch = head.localEulerAngles.x;
            float pitchDelta = Mathf.DeltaAngle(currentPitch, targetPitch);
            //Debug.Log(currentPitch + " " + targetPitch + " " + pitchDelta);
            Quaternion rotation = Quaternion.Euler(pitchDelta, 0f, 0f);
            Quaternion baseHead = head.localRotation;
            head.localRotation = baseHead * rotation;
            Quaternion totalRotation = head.localRotation * Quaternion.Inverse(headBaseRotation);
            if (leftArm != null)
            {
                Quaternion baseLeft = leftArm.localRotation;
                leftArm.localRotation = baseLeft * totalRotation;
                //Debug.Log($"rotating left {rotation} {baseLeft} {leftArm.localRotation}");
            }
            if (rightArm != null)
            {
                Quaternion baseRight = rightArm.localRotation;
                rightArm.localRotation = baseRight * totalRotation;
            }
            
            //Debug.Log($"rotating head {rotation} {baseHead} {head.localRotation}");
        }
        
    }

    /// <summary>
    /// Initializes the zombie's targets.
    /// </summary>
    /// <param name="player">Player GameObject</param>
    /// <param name="constructTargets">List of constructs</param>
    public void SetTargets(Construct[] constructTargets)
    {
        this.constructTargets = constructTargets;
        initialized = true;
    }

    /// <summary>
    /// Handles choosing the target and attacking (to be overriden by zombie variants)
    /// </summary>
    /// <returns>Whether or not the target was changed</returns>
    protected virtual bool HandleAttack()
    {
        bool changedTarget = false;
        //Debug.Log($"Handle attack: {attack.IsAttacking}");
        if (!attack.IsAttacking)
        {
            moveEnabled = true; // not attacking currently, so move is allowed
            // Choose target and attack if in range
            changedTarget = ChooseTarget();
            if (changedTarget)
            {
                zombieTarget = target.GetComponent<ZombieTarget>();
                constructCollider = zombieTarget.ConstructCollider;
            }
            if (GetDistanceSquared(target) <= (attackRange + zombieTarget.Radius) * (attackRange + zombieTarget.Radius) &&
                    (target != player || (target == player && Player.Instance.Health.IsAlive)))
            {
                bool success = attack.Attack(zombieTarget);
                if (success && attack.DisableMovement)
                {
                    moveEnabled = false; // disable movement when beginning attack if applicable
                }
            }
        }
        return changedTarget;
    }

    /// <summary>
    /// Chooses the zombie's target based on distance.
    /// </summary>
    /// <returns>Whether or not the target changed</returns>
    protected bool ChooseTarget()
    {
        GameObject closestConstruct = null;
        Construct closest = null;
        float closestDist = float.MaxValue;
        foreach (Construct c in constructTargets)
        {
            if (!c.IsAlive || !c.IsActive) continue;
            float dist = GetDistanceSquared(c.gameObject);
            if (dist < closestDist)
            {
                closestConstruct = c.gameObject;
                closestDist = dist;
                closest = c;
            }
        }

        // Player distance
        float playerDist = GetDistanceSquared(player);
        if (!Player.Instance.Health.IsAlive || playerDist > playerPriorityRange)
        {
            playerDist = float.MaxValue;
        }
        else
        {
            // Check kite range (must not be too close to construct target)
            if (closestDist >= kiteRangeThreshold * kiteRangeThreshold)
            {
                // The farther it gets from the construct, the smaller its kite range is
                float kiteRange = maxKiteRange * (kiteRangeThreshold / Mathf.Sqrt(closestDist));
                if (playerDist > kiteRange)
                {
                    playerDist = float.MaxValue;
                }
            }
            else
            {
                playerDist = float.MaxValue; // too close to construct; will only target construct now
            }
            
        }
        bool changed = false;
        if (playerDist <= closestDist || (attack.GetNumAttachedArms() == 0 && Player.Instance.Health.IsAlive)) // if no arms, follow player
        {
            changed = (target == null || !target.Equals(player));
            target = player;
            agent.stoppingDistance = playerStoppingDist;
        }
        else
        {
            changed = (target == null || !target.Equals(closestConstruct));
            target = closestConstruct;
            if (changed)
            {
                switch (closest.GetConstructType())
                {
                    case Construct.Type.DISRUPTOR:
                        agent.stoppingDistance = disruptorStoppingDist;
                        break;
                    case Construct.Type.DEFENDER:
                        agent.stoppingDistance = defenderStoppingDist;
                        break;
                    case Construct.Type.BEACON:
                        agent.stoppingDistance = beaconStoppingDist;
                        break;
                }
            }
        }
        return changed;
    }

    /// <summary>
    /// Calculates the horizontal distance between the zombie and the given target.
    /// </summary>
    /// <param name="target">The target to calculate the distance to</param>
    /// <returns>The distance to the target</returns>
    protected float GetDistanceSquared(GameObject target)
    {
        Vector3 delta = target.transform.position - transform.position;
        delta.y = 0;
        return delta.sqrMagnitude;
    }
}
