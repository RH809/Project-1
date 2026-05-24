/// <summary>
/// This script controls the pathfinding of a zombie.
/// </summary>

using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private Animator zombieAnimator;
    private NavMeshAgent agent;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float playerStoppingDist;
    [SerializeField] private float disruptorStoppingDist;
    [SerializeField] private float maxKiteRange; // kite range will scale inversely with distance from closest constructs
    [SerializeField] private float kiteRangeThreshold; // construct target must be outside of this range for player to take aggro
    [SerializeField] private float playerPriorityRange; // will not prioritize player if it is out of this range

    [SerializeField] private Transform zombieHead;

    private ZombieAttack attack;
    private Rigidbody rb;

    // Given by spawner
    private GameObject player;
    private GameObject[] constructTargets;

    private GameObject target;

    private bool initialized = false; 
    private bool moveEnabled = true;
    private bool wasMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attack = GetComponent<ZombieAttack>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!initialized) return; // check that targets have been initialized
        bool changedTarget = false;
        if (!attack.IsAttacking)
        {
            // Choose target and attack if in range
            changedTarget = ChooseTarget();
            if (GetDistance(target) <= attackRange)
            {
                attack.Attack();
            }
        }
        
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
                Debug.Log("Starting movement toward " + destination + " | " + agent.destination);
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
                    Debug.Log("Updating player tracking");
                    //Debug.Log("Updating player tracking " + destination + " | " + agent.destination);
                    agent.SetDestination(destination);
                }
                if (wasMoving)
                {
                    if (stopped)
                    {
                        Debug.Log("Stopping movement animation");
                        zombieAnimator.ResetTrigger("Start Moving");
                        zombieAnimator.SetTrigger("Stop Moving");
                        wasMoving = false;
                    }
                    else
                    {
                        /*
                        if (target.Equals(player))
                        {
                            Vector3 destination = player.transform.position;
                            destination.y = rb.transform.position.y;
                            Debug.Log("Updating player tracking");
                            //Debug.Log("Updating player tracking " + destination + " | " + agent.destination);
                            agent.SetDestination(destination);
                        }
                        else
                        {
                            Debug.Log($"Still tracking {target}");
                        }
                        */
                        Debug.Log("Still moving");
                        wasMoving = true;
                    }
                }
                else
                {
                    if (!stopped)
                    {
                        Debug.Log("Starting movement from stop");
                        zombieAnimator.ResetTrigger("Stop Moving");
                        zombieAnimator.SetTrigger("Start Moving");
                        wasMoving = true;
                    }
                    else
                    {
                        Debug.Log("Still not moving");
                        wasMoving = false;
                    }
                }
            }
            /*
            float distanceToTarget = GetDistance(target);
            Debug.Log(agent.speed + " " + agent.isStopped + " " + wasMoving);
            if (wasMoving && agent.isStopped) //distanceToTarget <= minTargetDist)
            {
                // Stop movement if within min distance
                //agent.ResetPath();
                Debug.Log("Stopping movement animation since too close");
                zombieAnimator.SetTrigger("Stop Moving");
                wasMoving = false;
            }
            else
            {
                if ((!wasMoving || changedTarget))
                {
                    // Start movement toward target if either wasn't moving or changed target and outside of min target distance
                    
                    Vector3 destination = target.transform.position;
                    destination.y = rb.transform.position.y;
                    agent.SetDestination(destination);
                    Debug.Log("Starting movement toward " + destination + " | " + agent.destination);
                    zombieAnimator.SetTrigger("Start Moving");
                }
                else if (target.Equals(player))
                { // Update target path if its player
                    Vector3 destination = player.transform.position;
                    destination.y = rb.transform.position.y;
                    Debug.Log("Updating player tracking " + destination + " | " + agent.destination);
                    agent.SetDestination(destination);
                }
                wasMoving = true;
            }
            */
            
        }
        else
        {
            if (wasMoving)
            {
                // Stop movement entirely
                Debug.Log("Stopping movement");
                agent.ResetPath();
                zombieAnimator.ResetTrigger("Start Moving");
                zombieAnimator.SetTrigger("Stop Moving");
            }
            wasMoving = false;
        }

        // rotate to face target even if not moving
        //Debug.Log($"Target: {target}  {target.transform.position}  {agent.destination}");
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
        }
        /*
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - rb.transform.position, Vector3.up);
        rb.MoveRotation(rotation);
        */
        Debug.DrawRay(rb.transform.position, agent.destination - rb.transform.position, Color.red);
        Debug.DrawRay(rb.transform.position, target.transform.position - rb.transform.position, Color.orange);
        /*
        // handle movement
        Vector3 targetPos = agent.steeringTarget;
        Vector3 dir = (targetPos - rb.position).normalized;

        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
        */
    }

    /// <summary>
    /// Initializes the zombie's targets.
    /// </summary>
    /// <param name="player">Player GameObject</param>
    /// <param name="constructTargets">List of construct GameObjects</param>
    public void SetTargets(GameObject player, GameObject[] constructTargets)
    {
        this.player = player;
        this.constructTargets = constructTargets;
        initialized = true;
    }

    /// <summary>
    /// Chooses the zombie's target based on distance.
    /// </summary>
    /// <returns>Whether or not the target changed</returns>
    bool ChooseTarget()
    {
        GameObject closestConstruct = null;
        float closestDist = float.MaxValue;
        foreach (GameObject c in constructTargets)
        {
            if (!c.GetComponent<Health>().IsAlive) continue;
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
            // Check if player is in view

            // Check kite range (must not be too close to construct target)
            if (closestDist > kiteRangeThreshold)
            {
                float kiteRange = maxKiteRange * (kiteRangeThreshold / playerDist);
                if (playerDist > kiteRange)
                {
                    playerDist = float.MaxValue;
                }
            }
            
        }
        bool changed = false;
        if (playerDist <= closestDist)
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
        }
        return changed;
    }

    /// <summary>
    /// Calculates the horizontal distance between the zombie and the given target.
    /// </summary>
    /// <param name="target">The target to calculate the distance to</param>
    /// <returns>The distance to the target</returns>
    float GetDistance(GameObject target)
    {
        float targetX = target.transform.position.x;
        float targetZ = target.transform.position.z;

        return Mathf.Sqrt(Mathf.Pow(transform.position.x - targetX, 2) + Mathf.Pow(transform.position.z - targetZ, 2));
    }

    public GameObject GetTarget() {
        return target;
    }

    public void DisableMovement()
    {
        moveEnabled = false;
    }

    public void EnableMovement()
    {
        moveEnabled = true;
    }
}
