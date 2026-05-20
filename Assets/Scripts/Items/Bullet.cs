using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    private Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * bulletSpeed * Time.fixedDeltaTime);
        //transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Collided with " + collision.collider);
        Destroy(gameObject);
    }
}
