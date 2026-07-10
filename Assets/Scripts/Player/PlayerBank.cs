using UnityEngine;

public class PlayerBank : MonoBehaviour
{
    [SerializeField] private int startingAmount;
    private int amount;

    public int Amount { get => amount; }
    void Start()
    {
        amount = startingAmount;
    }

    void Update()
    {
        if (GameManager.Instance.DEBUG && Input.GetKeyDown("m"))
        {
            AddMoney(100);
        }
    }

    public void AddMoney(int addAmount)
    {
        amount += addAmount;
    }

    public void RemoveMoney(int removeAmount)
    {
        amount -= removeAmount;
    }
}
