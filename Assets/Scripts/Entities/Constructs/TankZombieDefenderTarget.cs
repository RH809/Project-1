/// <summary>
/// This script gives information about the tank zombie to the defender.
/// </summary>
using UnityEngine;

public class TankZombieDefenderTarget : DefenderTarget
{
    public override Vector3 GetDefenderTarget()
    {
        return transform.position + transform.rotation * offset;
    }
}
