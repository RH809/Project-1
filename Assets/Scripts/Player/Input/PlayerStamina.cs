/// <summary>
/// This script handles the behavior for the player's stamina.
/// </summary>
using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private float staminaRegenRate;
    [SerializeField] private float staminaUseRate;
    private float currentStamina;
    public float CurrentStamina { get => currentStamina; }
    public float MaxStamina { get => 100; }

    public bool CanSprint { get => !disableSprint && CurrentStamina > 0; }

    private bool disableSprint = false;

    public bool SprintDisabled { get => disableSprint; }

    void Start()
    {
        currentStamina = MaxStamina;
    }

    void OnEnable()
    {
        Health.OnDie += OnDie;
        Health.OnRespawn += OnRespawn;
    }

    void OnDisable()
    {
        Health.OnDie -= OnDie;
        Health.OnRespawn -= OnRespawn;
    }

    void Update()
    {
        if (Player.Instance.Health.IsAlive)
        {
            if (!disableSprint && Player.Instance.Movement.IsMoving && Player.Instance.Movement.IsSprinting)
            {
                currentStamina = Mathf.Max(0, currentStamina - staminaUseRate * Time.deltaTime);
                if (currentStamina == 0)
                {
                    StartCoroutine(DisableSprintRoutine());
                }
            }
            else
            {
                currentStamina = Mathf.Min(MaxStamina, currentStamina + staminaRegenRate * Time.deltaTime);
            }
        }
    }

    IEnumerator DisableSprintRoutine()
    {
        disableSprint = true;
        yield return new WaitUntil(() => currentStamina >= MaxStamina / 2);
        disableSprint = false;
    }

    public void OnDie(HealthContext healthContext)
    {
        if (healthContext.target.Equals(Player.Instance.gameObject))
        {
            currentStamina = 0;
        }
    }

    public void OnRespawn(HealthContext healthContext)
    {
        if (healthContext.target.Equals(Player.Instance.gameObject))
        {
            currentStamina = MaxStamina;
        }
    }
}
