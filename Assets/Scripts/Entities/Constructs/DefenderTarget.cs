/// <summary>
/// This script gives information about the defender's target.
/// </summary>
using UnityEngine;

public class DefenderTarget : MonoBehaviour
{
    [SerializeField] protected Vector3 offset;

    public virtual Vector3 GetDefenderTarget()
    {
        return transform.position + offset;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GetDefenderTarget(), 0.2f);
    }
}
