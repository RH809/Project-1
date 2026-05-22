using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private Animator zombieAnimator;

    [SerializeField] private int damage;
    [SerializeField] private bool disableMovement;

    private static string attackTriggerName = "Zombie Attack";

    private bool attacking = false;
    public bool IsAttacking { get => attacking; }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (attacking) return;
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
