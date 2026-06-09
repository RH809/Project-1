/// <summary>
/// This script handles the movement and collision of the bullets.
/// </summary>

using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float maxRange = 50f;
    [SerializeField] private float bulletRadius = 0.01f;
    [SerializeField] private LayerMask collisionMask;

    private GameObject[] disruptors;

    private Rigidbody rb;
    private bool hasHit = false;

    private Vector3 startPos;
    void Start() {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    public void SetDisruptors(GameObject[] disruptors) {
        this.disruptors = disruptors;
    }

    void FixedUpdate()
    {

        Vector3 currentPos = rb.position;
        Vector3 newPos = rb.position + transform.forward * bulletSpeed * Time.fixedDeltaTime;
        // Check for collision along movement
        Collider[] overlaps = Physics.OverlapCapsule(
            currentPos,
            newPos,
            bulletRadius,
            collisionMask
        );
        if (overlaps.Length > 0) {
            // Find the collision that happened closest to the previous position along the path
            Collider closest = overlaps[0];
            Vector3 closestDist = closest.ClosestPoint(currentPos);
            foreach (var c in overlaps)
            {
                Vector3 hitPoint = c.ClosestPoint(currentPos);
                Vector3 hitDist = currentPos - hitPoint;
                Debug.Log("Bullet Collision: " + c.gameObject + " " + hitDist.magnitude + " " + closestDist.magnitude);
                if (hitDist.magnitude < closestDist.magnitude)
                {
                    closest = c;
                    closestDist = hitDist;
                }
            }
            Collide(closest.gameObject, closest.ClosestPoint(currentPos));
        }
        
        rb.MovePosition(newPos);
        Vector3 dist = rb.position - startPos;
        if (dist.magnitude >= maxRange) { // Destory if past max range
            Destroy(gameObject);
        }
        //transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Handles the collision with another GameObject.
    /// </summary>
    /// <param name="other">The GameObject it collided with</param>
    /// <param name="collisionPoint">Where it collided</param>
    void Collide(GameObject other, Vector3 collisionPoint) {
        if (hasHit) return;
        // Handle actual hitbox of walls
        if (other.layer == LayerMask.NameToLayer("Walls") && collisionPoint.y > Shoot.wallHeight)
        {
            return;
        }
        foreach (GameObject obj in disruptors)
        {
            // Handle lower hitbox of dead disruptors
            if (obj.Equals(other) && !obj.GetComponent<Disruptor>().IsAlive && collisionPoint.y > Shoot.adjustedDisruptorHeight)
            {
                return;
            }
        }
        Debug.Log("Collided with " + other);
        ZombieBodyPart bodyPart = other.transform.gameObject.GetComponent<ZombieBodyPart>();
        if (bodyPart != null)
        {
            // If it hit a zombie's body part, deal the damage
            bodyPart.TakeDamage(damage, gameObject);
        }
        hasHit = true;
        Destroy(gameObject);
    }
}
