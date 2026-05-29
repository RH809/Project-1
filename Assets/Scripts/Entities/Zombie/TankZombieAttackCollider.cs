using System.Collections;
using UnityEngine;

public class TankZombieAttackCollider : MonoBehaviour
{
    [SerializeField] private Vector3 endScale;
    [SerializeField] private float duration;
    [SerializeField] private float startOuterRadius;
    [SerializeField] private float startInnerRadius;

    private float outerRadius;
    private float innerRadius;
    void Start()
    {
        StartCoroutine(Grow());
    }

    /// <summary>
    /// Grows the attack collider
    /// </summary>
    IEnumerator Grow()
    {
        Vector3 start = transform.localScale;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            transform.localScale = Vector3.Lerp(start, endScale, lerp);
            // Calculate current radii
            outerRadius = startOuterRadius * transform.localScale.x / start.x;
            innerRadius = startInnerRadius * transform.localScale.x / start.x;

            yield return null;
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
