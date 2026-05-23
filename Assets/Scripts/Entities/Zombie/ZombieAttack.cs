/// <summary>
/// This script handles the attack state of the zombies.
/// </summary>
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private Animator zombieAnimator;
    [SerializeField] private ZombieArm leftArm;
    [SerializeField] private ZombieArm rigthArm;
    [SerializeField] private GameObject attackCollider;
    private Zombie zombie;

    [SerializeField] private int damage;
    [SerializeField] private bool disableMovement;

    private static string attackTriggerName = "Zombie Attack";

    private bool attacking = false;
    public bool IsAttacking { get => attacking; }

    void Start()
    {
        zombie = GetComponent<Zombie>();
        attackCollider.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Attack();
        }
    }

    /// <summary>
    /// Gets the number of attached arms to determine attack damage and whether the zombie can attack
    /// </summary>
    /// <returns>The number of attached arms</returns>
    int GetNumAttachedArms()
    {
        int num = 0;
        if (leftArm.Attached) num++;
        if (rigthArm.Attached) num++;
        return num;
    }

    public void Attack()
    {
        if (attacking || GetNumAttachedArms() == 0) return;
        attacking = true;
        zombieAnimator.SetTrigger(attackTriggerName);
    }

    public void ZombieAttackStart()
    {
        Debug.Log("zombie attack start");
        attackCollider.SetActive(true);
    }

    public void ZombieAttackEnd()
    {
        Debug.Log("zombie attack end");
        attacking = false;
        attackCollider.SetActive(false);
    }
}
