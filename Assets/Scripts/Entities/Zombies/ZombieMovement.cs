/// <summary>
/// This script controls the pathfinding of a zombie.
/// </summary>

using System;
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
    [SerializeField] private float maxKiteRange; // kite range will scale inversely with distance from closest constructs
    [SerializeField] private float kiteRangeThreshold; // construct target must be outside of this range for player to take aggro
    [SerializeField] private float playerPriorityRange; // will not prioritize player if it is out of this range

    [SerializeField] private Transform head;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    private Quaternion headBaseRotation;

    protected ZombieAttack attack;
    protected Rigidbody rb;

    
    protected GameObject player;
    protected GameObject[] constructTargets; // Given by spawner

    protected GameObject target;
    protected ZombieTarget zombieTarget;

    protected bool initialized = false; 
    protected bool moveEnabled = true;
    protected bool rbRotation = true;
    private bool wasMoving = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        attack = GetComponent<ZombieAttack>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.speed = moveSpeed;
        player = Player.Instance.gameObject;

        headBaseRotation = head.localRotation;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (!initialized) return; // check that targets have been initialized
        bool changedTarget = HandleAttack();
        //Debug.Log($"{moveEnabled} {attack.IsAttacking}");
        if (moveEnabled)
        {
            // move toward target if necessary and allowed
            //rb.MovePosition(rb.position + rb.transform.forward * moveSpeed * Time.fixedDeltaTime);
            
            bool stopped = agent.velocity.magnitude < 0.01f;
            //Debug.Log($"{target} {wasMoving} {agent.isStopped} {stopped} {changedTarget}");
            if (changedTarget)
            {
                Vector3 destination = target.transform.position;
                destination.y = rb.transform.position.y;
                agent.SetDestination(destination);
                //Debug.Log("Starting movement toward " + destination + " | " + agent.destination);
                zombieAnimator.ResetTrigger("Stop Moving");
                zombieAnimator.SetTrigger("Start Moving");
                wasMoving = true;
            }
            else
            {
                if (target.Equals(player))
                {
                    Vector3 destination = player.transform.position;
                    destination.y = rb.transform.position.y;
                    //Debug.Log("Updating player tracking");
                    //Debug.Log("Updating player tracking " + destination + " | " + agent.destination);
                    agent.SetDestination(destination);
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

        // rotate to face target even if not moving
        //Debug.Log($"Target: {target}  {target.transform.position}  {agent.destination}");
        if (!rbRotation) return;
        Vector3 dir = target.transform.position - rb.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
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
        Debug.DrawRay(rb.transform.position, agent.destination - rb.transform.position, Color.red);
        Debug.DrawRay(rb.transform.position, target.transform.position - rb.transform.position, Color.orange);

        
    }

    protected virtual void LateUpdate()
    {
        // rotate arms and head to point toward target
        if (zombieTarget)
        {
            Vector3 targetPoint = zombieTarget.GetZombieTarget();
            Vector3 direction = targetPoint - head.position;
            Debug.DrawRay(head.position, direction);
            float targetPitch = -Mathf.Atan2(
                direction.y,
                new Vector2(direction.x, direction.z).magnitude
            ) * Mathf.Rad2Deg;
            float currentPitch = head.localEulerAngles.x;
            float pitchDelta = Mathf.DeltaAngle(currentPitch, targetPitch);
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
    /// <param name="constructTargets">List of construct GameObjects</param>
    public void SetTargets(GameObject[] constructTargets)
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
            }
            if (GetDistance(target) <= attackRange + zombieTarget.Radius &&
                    (target != player || (target == player && Player.Instance.Health.IsAlive)))
            {
                bool success = attack.Attack();
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
        float closestDist = float.MaxValue;
        foreach (GameObject c in constructTargets)
        {
            if (!c.GetComponent<Health>().IsAlive || !c.GetComponent<Construct>().IsActive) continue;
            float dist = GetDistance(c);
            if (dist < closestDist)
            {
                closestConstruct = c;
                closestDist = dist;
            }
        }

        // Player distance
        float playerDist = GetDistance(player);
        if (!player.GetComponent<Health>().IsAlive || playerDist > playerPriorityRange)
        {
            playerDist = float.MaxValue;
        }
        else
        {
            // Check kite range (must not be too close to construct target)
            if (closestDist >= kiteRangeThreshold)
            {
                // The farther it gets from the construct, the smaller its kite range is
                float kiteRange = maxKiteRange * (kiteRangeThreshold / closestDist);
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
        if (playerDist <= closestDist || attack.GetNumAttachedArms() == 0) // if no arms, follow player
        {
            changed = (target == null || !target.Equals(player));
            target = player;
            agent.stoppingDistance = playerStoppingDist;
        }
        else
        {
            changed = (target == null || !target.Equals(closestConstruct));
            target = closestConstruct;
            if (target.GetComponent<Disruptor>() != null)
            {
                agent.stoppingDistance = disruptorStoppingDist;
            }
            else if (target.GetComponent<Defender>() != null)
            {
                agent.stoppingDistance = defenderStoppingDist;
            }
        }
        return changed;
    }

    /// <summary>
    /// Calculates the horizontal distance between the zombie and the given target.
    /// </summary>
    /// <param name="target">The target to calculate the distance to</param>
    /// <returns>The distance to the target</returns>
    protected float GetDistance(GameObject target)
    {
        float targetX = target.transform.position.x;
        float targetZ = target.transform.position.z;

        return Mathf.Sqrt(Mathf.Pow(transform.position.x - targetX, 2) + Mathf.Pow(transform.position.z - targetZ, 2));
    }
}
