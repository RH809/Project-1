/// <summary>
/// This script gives information about the zombie's target.
/// </summary>

using UnityEngine;

public class ZombieTarget : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float radius; // radius of the collider of the target
    [SerializeField] private Vector3 bottomOffset; // bottom of the hitbox
    public float Radius { get => radius;  }

    public Vector3 GetZombieTarget()
    {
        return transform.position + offset;
    }

    public virtual Vector3 GetHitboxBottom()
    {
        return transform.position + bottomOffset;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(GetHitboxBottom(), 0.3f);
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(GetZombieTarget(), 0.2f);
        Gizmos.color = Color.pink;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
