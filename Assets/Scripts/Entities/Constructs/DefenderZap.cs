using UnityEngine;

public class DefenderZap : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    private DefenderTarget target;
    private float damage;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(DefenderTarget target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.GetDefenderTarget() - rb.position;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
    
    }
}
