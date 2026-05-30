/// <summary>
/// This script listens for the tank zombie animation events and passes it on to the
/// scripts in the parent object.
/// </summary>
using UnityEngine;

public class TankZombieAnimation : MonoBehaviour
{
    [SerializeField] private TankZombieAttack tankZombieAttack;

    public void ZombieAttackStart()
    {
        tankZombieAttack.ZombieAttackStart();
    }

    public void ZombieAttackEnd()
    {
        tankZombieAttack.ZombieAttackEnd();
    }
}
