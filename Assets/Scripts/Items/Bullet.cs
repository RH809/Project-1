using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float maxRange = 50f;
    [SerializeField] private float bulletRadius = 0.01f;
    [SerializeField] private LayerMask collisionMask;

    private GameObject[] disruptors;

    private Rigidbody rb;

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
            Collider closest = overlaps[0];
            Vector3 closestDist = currentPos = closest.ClosestPoint(currentPos);
            foreach (var c in overlaps)
            {
                Vector3 hitPoint = c.ClosestPoint(currentPos);
                Vector3 hitDist = currentPos - hitPoint;
                if (hitDist.magnitude < closestDist.magnitude) {
                    closest = c;
                    closestDist = hitDist;
                }
            }
            Collide(closest.gameObject, closest.ClosestPoint(currentPos));
        }
        
        rb.MovePosition(newPos);
        Vector3 dist = rb.position - startPos;
        if (dist.magnitude >= maxRange) {
            Destroy(gameObject);
        }
        //transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        Collide(collision.collider.gameObject, collision.GetContact(0).point);
    }

    void Collide(GameObject other, Vector3 collisionPoint) {
        foreach (GameObject obj in disruptors)
        {
            // Handle different hitbox of dead disruptors
            if (obj.Equals(other) && !obj.GetComponent<Disruptor>().isAlive && collisionPoint.y > 0.25)
            {
                return;
            }
        }
        Debug.Log("Collided with " + other);
        Destroy(gameObject);
    }
}
