/// <summary>
/// This script manages the timing of the wave spawns and the game UI.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public static float GroundY = -0.5f;

    [SerializeField] private int waves = 30;
    [SerializeField] private int waveSpawnInterval = 30;
    [SerializeField] private ZombieSpawner leftZombieSpawner;
    [SerializeField] private ZombieSpawner rightZombieSpawner;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI announcementText;
    [SerializeField] private TextMeshProUGUI zombiesRemainingText;

    [SerializeField] private float announcementTime;

    private int waveNum;
    public int WaveNum { get => waveNum; }
    private int countdown;

    private Coroutine announcementCoroutine;
    private Queue<string> announcements;

    void Start()
    {
        announcements = new Queue<string>();
        announcementText.text = "";
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        waveText.text = $"Wave {waveNum} spawning in {countdown}";
        zombiesRemainingText.text = "Zombies Remaining: " + DefenderManager.Instance.NumZombies.ToString();
        if (announcementCoroutine == null && announcements.Count > 0)
        {
            announcementCoroutine = StartCoroutine(ShowAnnouncement(announcements.Dequeue()));
        }
    }

    IEnumerator SpawnWaves()
    {
        for (waveNum = 1; waveNum <= waves; waveNum++)
        {
            for (countdown = waveSpawnInterval; countdown > 0; countdown--)
            {
                yield return new WaitForSeconds(1);
            }
            leftZombieSpawner.WaveSetup();
            rightZombieSpawner.WaveSetup();
            Debug.Log("Spawning wave...");
            leftZombieSpawner.SpawnWave();
            rightZombieSpawner.SpawnWave();
        }
    }

    public void AddAnnouncement(string announcement)
    {
        announcements.Enqueue(announcement);
    }

    IEnumerator ShowAnnouncement(string announcement)
    {
        announcementText.text = announcement;
        yield return new WaitForSeconds(announcementTime);
        announcementText.text = "";
        announcementCoroutine = null;
    }
}
