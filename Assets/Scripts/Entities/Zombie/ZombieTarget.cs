/// <summary>
/// This script gives the target location for where the zombie should look at the target.
/// </summary>

using UnityEngine;

public class ZombieTarget : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    public Vector3 GetZombieTarget()
    {
        return transform.position + offset;
    }
}
