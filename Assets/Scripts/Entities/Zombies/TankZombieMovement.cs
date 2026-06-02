/// <summary>
/// This script handles the movement for the tank zombie variant. It adds rotation
/// toward target during LateUpdate to ZombieMovement since animations for the
/// tank zombie rotate the body.
/// </summary>
using UnityEngine;

public class TankZombieMovement : ZombieMovement
{
    private Transform parentTransform;
    protected override void Start()
    {
        base.Start();
        rbRotation = false;
        parentTransform = transform.parent;
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        
        if (!initialized || !moveEnabled || target == null) return; // check that targets have been initialized
        // Rotate toward target during late update since animation rotates body
        //parentTransform.position += rb.transform.localPosition;
        //rb.transform.localPosition = Vector3.zero;
        //parentTransform.position = rb.transform.position;
        Vector3 dir = target.transform.position - rb.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(
                rb.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            ));
        }
        
    
    }
}
