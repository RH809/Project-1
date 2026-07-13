/// <summary>
/// This script handles the behavior of the zombie when they are stunned.
/// </summary>
using System.Collections;
using UnityEngine;

public class StunVictim : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackCollider;
    private bool stunned = false;
    public bool Stunned { get => stunned; }

    private float stunDuration;
    private Coroutine stunRoutine;
    private bool wasAttacking = false;

    public void Stun(float stunDuration)
    {
        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
        }
        this.stunDuration = stunDuration;
        stunned = true;
        animator.speed = 0.0f;
        wasAttacking = (attackCollider != null && attackCollider.activeInHierarchy);
        if (wasAttacking)
        {
            attackCollider.SetActive(false);
        }
        stunRoutine = StartCoroutine(StunRoutine());
    }

    IEnumerator StunRoutine()
    {
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
        animator.speed = 1.0f;
        if (wasAttacking)
        {
            attackCollider.SetActive(true);
        }
        stunRoutine = null;
    }

    public void Reset()
    {
        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
        }
        stunned = false;
        animator.speed = 1.0f;
    }
}
