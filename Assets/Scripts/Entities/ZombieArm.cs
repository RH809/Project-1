using UnityEngine;

public class ZombieArm : MonoBehaviour
{
    [SerializeField] private int armHealth;

    private int currentHealth;
    private bool attached = true;

    public bool Attached { get => attached; }

    void Start()
    {
        currentHealth = armHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            attached = false;
            Destroy(gameObject);
        }
    }
}
