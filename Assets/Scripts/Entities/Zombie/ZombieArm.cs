/// <summary>
/// This script handles the "health" of a zombie's arm and handle the mechanic of destroying the zombie's arm
/// </summary>

using UnityEngine;

public class ZombieArm : MonoBehaviour
{
    [SerializeField] private int armHealth;

    private float currentHealth;
    private bool attached = true;

    public bool Attached { get => attached; }

    void Start()
    {
        currentHealth = armHealth;
    }
    public void TakeDamage(float damage)
    {
        Debug.Log($"Arm took {damage} damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Destroy the arm
            attached = false;
            //Destroy(gameObject);
            Debug.Log("Deactivating arm");
            gameObject.SetActive(false);
        }
    }
}
