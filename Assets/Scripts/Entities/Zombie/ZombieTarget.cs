/// <summary>
/// This script gives the target location for where the zombie should look at the target.
/// </summary>

using UnityEngine;

public class ZombieTarget : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float radius; // radius of the collider of the target
    public float Radius { get => radius;  }

    public Vector3 GetZombieTarget()
    {
        return transform.position + offset;
    }
}
