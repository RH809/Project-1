/// <summary>
/// This script handles the grenade throwing.
/// </summary>
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    [SerializeField] private Transform grenadeThrowPoint;

    [SerializeField] private GameObject grenade;

    private Aim aim;
    private bool throwingGrenade = false;
    public bool ThrowingGrenade { get => throwingGrenade; }

    void Start()
    {
        aim = GetComponent<Aim>();
    }

    public void GrenadeRelease()
    {
        // create grenade
        ThrowGrenade();
        Player.Instance.Inventory.UseGrenade();
        throwingGrenade = true;
    }
    private void ThrowGrenade()
    {
        Quaternion aimRotation = aim.GetTargetRotation(grenadeThrowPoint);
        GameObject newGrenade = Instantiate(grenade, grenadeThrowPoint.position, aimRotation);
    }

    public void GrenadeThrowEnd()
    {
        throwingGrenade = false;
    }
}
