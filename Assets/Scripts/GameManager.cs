/// <summary>
/// This script manages the timing of the wave spawns and the game UI.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public static float GroundY = -0.5f;

    [SerializeField] private GameObject beacon;
    private bool gameOver = false;
    public bool GameOver { get => gameOver; }

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
        announcementText.alpha = 0;
        waveNum = 1;
        StartCoroutine(SpawnWaves());
    }

    private void OnEnable()
    {
        Health.OnDie += OnBeaconDeath;
    }

    private void OnDisable()
    {
       Health.OnDie -= OnBeaconDeath;
    }

    void Update()
    {
        waveText.text = $"Wave {waveNum} spawning in {countdown}";
        zombiesRemainingText.text = "Zombies Remaining: " + DefenderManager.Instance.NumZombies.ToString();
        if (announcementCoroutine == null && announcements.Count > 0)
        {
            announcementCoroutine = StartCoroutine(ShowAnnouncement(announcements.Dequeue()));
        }
        if (waveNum == waves && leftZombieSpawner.FinishedSpawning && rightZombieSpawner.FinishedSpawning && DefenderManager.Instance.NumZombies == 0)
        {
            EndGame(true);
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
            AddAnnouncement("New wave spawning");
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
        announcementText.alpha = 0;
        float t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            announcementText.alpha = Mathf.Lerp(0, 1, t / 0.5f);
            yield return null;
        }
        announcementText.alpha = 1;
        yield return new WaitForSeconds(announcementTime);
        t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            announcementText.alpha = Mathf.Lerp(1, 0, t / 0.5f);
            yield return null;
        }
        announcementText.alpha = 0;
        announcementText.text = "";
        announcementCoroutine = null;
    }

    void OnBeaconDeath(HealthContext healthContext)
    {
        if (healthContext.target == beacon)
        {
            EndGame(false);
        }
    }

    void EndGame(bool win)
    {
        gameOver = true;
        Player.Instance.Movement.StopMovement();
        if (win)
        {

        }
        else
        {
            
        }
    }
}
