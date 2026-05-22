using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private Animator zombieAnimator;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float minTargetDist;
    [SerializeField] private float maxKiteRange; // kite range will scale inversely with distance from closest constructs
    [SerializeField] private float kiteRangeThreshold;
    [SerializeField] private float playerPriorityRange;

    [SerializeField] private Transform zombieHead;

    private ZombieAttack attack;
    private Rigidbody rb;

    // Given by spawner
    private GameObject player;
    private GameObject[] constructTargets;

    private GameObject target;

    private bool initialized = false; 
    private bool moveEnabled = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attack = GetComponent<ZombieAttack>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!initialized) return; // check that targets have been initialized
        if (!attack.IsAttacking)
        {
            // Choose target and attack if in range
            ChooseTarget();
            if (GetDistance(target) <= attackRange)
            {
                attack.Attack();
            }
        }
        // rotate to face target
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - rb.transform.position, Vector3.up);
        rb.MoveRotation(rotation);
        if (moveEnabled && GetDistance(target) > minTargetDist)
        {
            // move toward target if necessary and allowed
            rb.MovePosition(rb.position + rb.transform.forward * moveSpeed * Time.fixedDeltaTime);
        }
        
    }

    public void SetTargets(GameObject player, GameObject[] constructTargets)
    {
        this.player = player;
        this.constructTargets = constructTargets;
        initialized = true;
    }

    void ChooseTarget()
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

            // Check kite range
            if (closestDist > kiteRangeThreshold)
            {
                float kiteRange = maxKiteRange * (kiteRangeThreshold / playerDist);
                if (playerDist > kiteRange)
                {
                    playerDist = float.MaxValue;
                }
            }
            
        }

        if (playerDist <= closestDist)
        {
            target = player;
        }
        else
        {
            target = closestConstruct;
        }
    }
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
