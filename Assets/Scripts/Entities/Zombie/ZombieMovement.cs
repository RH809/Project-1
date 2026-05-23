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
    [SerializeField] private float attackRange;
    [SerializeField] private float minTargetDist;
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
        agent.speed = moveSpeed;
        agent.stoppingDistance = minTargetDist;
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
            if (wasMoving && agent.isStopped)
            {
                // Stop movement animation but keep the tracking
                Debug.Log("Stopping movement");
                zombieAnimator.SetTrigger("Stop Moving");
                wasMoving = false;
            }
            else
            {
                if (!wasMoving || changedTarget)
                {
                    // Start movement toward target if either wasn't moving or changed target
                    
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
            
        }
        else
        {
            if (wasMoving)
            {
                // Stop movement entirely
                Debug.Log("Stopping movement");
                agent.ResetPath();
                zombieAnimator.SetTrigger("Stop Moving");
            }
            wasMoving = false;
        }
        // rotate to face target
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - rb.transform.position, Vector3.up);
        rb.MoveRotation(rotation);
        Debug.DrawRay(rb.transform.position, agent.destination - rb.transform.position, Color.red);
        Debug.DrawRay(rb.transform.position, target.transform.position - rb.transform.position, Color.orange);
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
        }
        else
        {
            changed = (target == null || !target.Equals(closestConstruct));
            target = closestConstruct;
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
