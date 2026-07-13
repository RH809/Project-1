/// <summary>
/// This script manages the spawn timing of the power up.
/// </summary>
using System.Collections;
using TMPro;
using UnityEngine;

public class PowerUpManager : Singleton<PowerUpManager>
{
    [SerializeField] private GameObject powerUp;
    [SerializeField] private int baseSpawnTime;
    public int SpawnTime { get => baseSpawnTime - Player.Instance.Boosts.AcceleratedAscension.CooldownDecrease; }
    [SerializeField] private TextMeshProUGUI spawnTimeText;
    private int t;
    private Coroutine spawnRoutine;
    void Start()
    {
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void Respawn()
    {
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        powerUp.SetActive(false);
        spawnTimeText.enabled = true;
        for (t = SpawnTime; t > 0; t--)
        {
            spawnTimeText.text = timeToText(t);
            yield return new WaitForSeconds(1);
        }
        spawnTimeText.enabled = false;
        powerUp.SetActive(true);
        GameManager.Instance.AddAnnouncement("Power Up Spawned");
        spawnRoutine = null;
    }

    public void DecreaseSpawnTime(int decreaseAmount)
    {
        if (spawnRoutine != null)
        {
            t -= decreaseAmount;
        }
    }

    string timeToText(int t)
    {
        return (t / 60).ToString() + ":" + (t % 60).ToString("D2");
    }
}
