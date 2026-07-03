/// <summary>
/// This script handles the stat multipliers and timing of the power up for the player.
/// </summary>
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerUp : MonoBehaviour
{
    [SerializeField] private float damageMultiplier;
    public float DamageMultiplier { get => damageMultiplier; }
    [SerializeField] private float speedMultiplier;

    public float SpeedMultiplier { get => speedMultiplier; }
    [SerializeField] private float duration;

    [SerializeField] private Image powerUpTimer;

    private bool active = false;
    public bool Active { get => active; }

    void Start()
    {
        powerUpTimer.fillAmount = 0;
    }

    public void Activate()
    {
        active = true;
        StartCoroutine(PowerUpRoutine());
    }

    IEnumerator PowerUpRoutine()
    {
        float t = 0;
        powerUpTimer.fillAmount = 1;
        while (t < duration)
        {
            t += Time.deltaTime;
            powerUpTimer.fillAmount = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        powerUpTimer.fillAmount = 0;
        active = false;
    }
}
