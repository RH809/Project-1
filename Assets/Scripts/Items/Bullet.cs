using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float maxRange = 50f;

    private Rigidbody rb;

    private Vector3 startPos;
    void Start() {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * bulletSpeed * Time.fixedDeltaTime);
        Vector3 dist = rb.position - startPos;
        if (dist.magnitude >= maxRange) {
            Destroy(gameObject);
        }
        //transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Collided with " + collision.collider);
        Destroy(gameObject);
    }
}
