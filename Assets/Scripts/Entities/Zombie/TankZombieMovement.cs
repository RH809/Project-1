/// <summary>
/// This script handles the movement for the tank zombie variant. It adds rotation
/// toward target during LateUpdate to ZombieMovement since animations for the
/// tank zombie rotate the body.
/// </summary>
using UnityEngine;

public class TankZombieMovement : ZombieMovement
{
    protected override void LateUpdate()
    {
        base.LateUpdate();
        /*
        // Rotate toward target during late update since animation rotates body
        if (!initialized) return; // check that targets have been initialized
        Vector3 dir = target.transform.position - rb.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            Quaternion rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.fixedDeltaTime
            );
            
            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRot,
                    rotationSpeed * Time.fixedDeltaTime
                )
            );
            
            Quaternion baseRotation = transform.localRotation;
            transform.localRotation = baseRotation * rotation;
        }
        */
    
    }
}
