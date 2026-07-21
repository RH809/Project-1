/// <summary>
/// This script handles the movement for the tank zombie variant. It adds rotation
/// toward target during LateUpdate to ZombieMovement since animations for the
/// tank zombie rotate the body.
/// </summary>
using UnityEngine;

public class TankZombieMovement : ZombieMovement
{
    protected override void Start()
    {
        base.Start();
        rbRotation = false;
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        
        if (!initialized || !moveEnabled || target == null) return; // check that targets have been initialized
        // Rotate toward target during late update since animation rotates body
        Vector3 dir = target.transform.position - rb.position;
        dir.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        rb.MoveRotation(Quaternion.Slerp(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        ));
        
    
    }
}
