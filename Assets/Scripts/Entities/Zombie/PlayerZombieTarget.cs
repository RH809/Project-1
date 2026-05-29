/// <summary>
/// This script gives information about the Player when being targeted by a zombie.
/// </summary>
using UnityEngine;

public class PlayerZombieTarget : ZombieTarget
{
    [SerializeField] private Vector3 jumpBottomOffset;

    public override Vector3 GetHitboxBottom()
    {
        if (Player.Instance != null && Player.Instance.Movement.IsJumping)
        {
            return transform.position + jumpBottomOffset;
        }
        else
        {
            return base.GetHitboxBottom();
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetComponent<Rigidbody>().position + jumpBottomOffset, 0.3f);
    }
}
