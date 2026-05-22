using JetBrains.Annotations;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private Animator zombieAnimator;
    [SerializeField] private ZombieArm leftArm;
    [SerializeField] private ZombieArm rigthArm;
    private Zombie zombie;

    [SerializeField] private int damage;
    [SerializeField] private bool disableMovement;

    private static string attackTriggerName = "Zombie Attack";

    private bool attacking = false;
    public bool IsAttacking { get => attacking; }

    void Start()
    {
        zombie = GetComponent<Zombie>();
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Attack();
        }
    }

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
    }

    public void ZombieAttackEnd()
    {
        Debug.Log("zombie attack end");
        attacking = false;
    }
}
