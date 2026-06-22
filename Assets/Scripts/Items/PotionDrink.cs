using UnityEngine;

public class PotionDrink : MonoBehaviour
{
    [SerializeField] private float healAmount;
    private Health health;

    private bool finishedDrinking = false;
    public bool FinishedDrinking { get => finishedDrinking; }

    void Start()
    {
        health = GetComponent<Health>();
    }

    public void PotionDrinkStart()
    {
        health.Heal(healAmount);
        Player.Instance.Inventory.UsePotion();
    }

    public void PotionDrinkFinish()
    {
        finishedDrinking = true;
    }

    public void PotionDrinkEnd()
    {
        finishedDrinking = false;
    }
}
