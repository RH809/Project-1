using UnityEngine;

public class DefenderZap : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject targetObject;
    private DefenderTarget target;
    private Health targetHealth;
    private float damage;
    private bool hasHit = false;

    public void Initialize(GameObject targetObject, DefenderTarget target, float damage)
    {
        this.targetObject = targetObject;
        targetHealth = targetObject.GetComponent<Health>();
        this.target = target;
        this.damage = damage;
    }

    void Update()
    {
        if (hasHit) return;
        if (targetObject != null && targetHealth.IsAlive && target != null)
        {
            Vector3 direction = target.GetDefenderTarget() - transform.position;
            if (direction.magnitude <= speed * Time.fixedDeltaTime)
            {
                transform.position = target.GetDefenderTarget();
                targetHealth.TakeDamage(damage, gameObject);
                hasHit = true;
                Destroy(gameObject);
            }
            else
            {
                transform.position += direction.normalized * speed * Time.fixedDeltaTime;
            }
        }
    }
}
