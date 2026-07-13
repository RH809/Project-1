using UnityEngine;
using UnityEngine.Pool;

public class DefenderZap : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject targetObject;
    private DefenderTarget target;
    private Health targetHealth;
    private float damage;
    private bool hasHit = false;

    private ObjectPool<DefenderZap> zapPool;

    public void Initialize(GameObject targetObject, DefenderTarget target, float damage)
    {
        hasHit = false;
        this.targetObject = targetObject;
        targetHealth = targetObject.GetComponent<Health>();
        this.target = target;
        this.damage = damage;
    }

    public void SetPool(ObjectPool<DefenderZap> zapPool)
    {
        this.zapPool = zapPool;
    }

    void Update()
    {
        if (hasHit) return;
        if (targetObject != null && targetHealth.IsAlive && target != null)
        {
            Vector3 direction = target.GetDefenderTarget() - transform.position;
            if (direction.magnitude <= speed * Time.deltaTime)
            {
                transform.position = target.GetDefenderTarget();
                targetHealth.TakeDamage(damage, gameObject);
                hasHit = true;
                //Destroy(gameObject);
                zapPool.Release(this);
            }
            else
            {
                transform.position += direction.normalized * speed * Time.deltaTime;
            }
        }
        else
        {
            //Destroy(gameObject);
            zapPool.Release(this);
        }
    }
}
