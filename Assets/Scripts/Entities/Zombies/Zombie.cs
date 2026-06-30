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
                Player.Instance.Bank.AddMoney(killReward);
                    Debug.Log("Creating money popup");
                    GameObject moneyPopup = Instantiate(moneyPopupPrefab, PlayerHUD.Instance.transform);
                    moneyPopup.GetComponent<TextMeshProUGUI>().text = "$" + killReward.ToString();
                    RectTransform popupRect = moneyPopup.GetComponent<RectTransform>();
                    do
                    {
                        popupRect.anchoredPosition = new Vector2(Random.Range(-70, 70), Random.Range(-40, 70)); // place popup randomly around center of the screen
                    } while (popupRect.anchoredPosition.magnitude < 25);
                    
            }
            health.DestroyHealthbar();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(contactPos, 0.5f);
    }
}
