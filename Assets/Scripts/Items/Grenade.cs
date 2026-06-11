using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float detonationTime;
    [SerializeField] private float throwForce;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 throwVector = rb.transform.forward * throwForce;
        rb.AddForce(throwVector, ForceMode.Impulse);
        StartCoroutine(DetonationTimer());
    }

    IEnumerator DetonationTimer()
    {
        yield return new WaitForSeconds(detonationTime);
        Detonate();
    }

    private void Detonate()
    {
        Debug.Log("Detonating...");
        Destroy(gameObject);
    }
}
