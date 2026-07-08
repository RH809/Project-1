/// <summary>
/// This script handles the zombie's death and acts as an identifying component.
/// </summary>

using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public enum ZombieType
    {
        TANK,
        REGULAR,
        MINI
    }

    [SerializeField] private ZombieType type;
    [SerializeField] private int killReward;
    [SerializeField] private GameObject moneyPopupPrefab;
    [SerializeField] private GameObject deathParticles;
    public ZombieType Type { get => type; }

    private Health health;
    private Vector3 contactPos;
    void Start()
    {
        health = GetComponent<Health>();
    }

    void OnEnable()
    {
        Health.OnDie += Die;
    }

    void OnDisable()
    {
        Health.OnDie -= Die;
    }

    void Die(HealthContext healthCtx)
    {
        if (healthCtx.target == gameObject)
        {
            Debug.Log("Zombie killed");
            if (healthCtx.source == Player.Instance.gameObject)
            {
                int moneyDrop = Mathf.CeilToInt(killReward * Player.Instance.Boosts.Collector.Multiplier);
                Player.Instance.Bank.AddMoney(moneyDrop);
                Debug.Log("Creating money popup");
                if (SettingsManager.Instance.ShowMoneyPopup)
                {
                    GameObject moneyPopup = Instantiate(moneyPopupPrefab, PlayerHUD.Instance.transform);
                    moneyPopup.GetComponent<TextMeshProUGUI>().text = "$" + moneyDrop.ToString();
                    RectTransform popupRect = moneyPopup.GetComponent<RectTransform>();
                    do
                    {
                        popupRect.anchoredPosition = new Vector2(Random.Range(-70, 70), Random.Range(-40, 70)); // place popup randomly around center of the screen
                    } while (popupRect.anchoredPosition.magnitude < 25);
                }  
            }
            health.DestroyHealthbar();
            if (SettingsManager.Instance.ShowDeathParticles)
            {
                DefenderTarget defenderTarget = GetComponent<DefenderTarget>();
                if (!defenderTarget) defenderTarget = GetComponentInChildren<DefenderTarget>();
                Instantiate(deathParticles, defenderTarget.GetDefenderTarget(), Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(contactPos, 0.5f);
    }
}
