/// <summary>
/// This script manages the spawn timing of the power up.
/// </summary>
using System.Collections;
using TMPro;
using UnityEngine;

public class PowerUpManager : Singleton<PowerUpManager>
{
    [SerializeField] private GameObject powerUp;
    [SerializeField] private int spawnTime;
    [SerializeField] private TextMeshProUGUI spawnTimeText;
    void Start()
    {
        StartCoroutine(SpawnTime());
    }

    public void Respawn()
    {
        StartCoroutine(SpawnTime());
    }

    IEnumerator SpawnTime()
    {
        powerUp.SetActive(false);
        spawnTimeText.enabled = true;
        for (int t = spawnTime; t > 0; t--)
        {
            spawnTimeText.text = timeToText(t);
            yield return new WaitForSeconds(1);
        }
        spawnTimeText.enabled = false;
        powerUp.SetActive(true);
        GameManager.Instance.AddAnnouncement("Power Up Spawned");
    }

    string timeToText(int t)
    {
        return (t / 60).ToString() + ":" + (t % 60).ToString("D2");
    }
}
