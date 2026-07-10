/// <summary>
/// This script manages the timing of the wave spawns and the game UI.
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    public static float GroundY = -0.5f;
    public bool DEBUG = false;

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

    [SerializeField] private Canvas gameOverUI;
    [SerializeField] private Image gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GameObject exitButton;

    [SerializeField] private GameObject gameOverCamera;
    [SerializeField] private Transform gameOverPosition;

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
        gameOverUI.enabled = false;
        gameOverCamera.SetActive(false);
        exitButton.GetComponent<Button>().onClick.AddListener(() => {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(0);
        });
        beacon.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;

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
        if (gameOver) return;
        if (waveNum <= waves) waveText.text = $"Wave {waveNum} spawning in {countdown}";
        else waveText.text = "Final Wave";
        zombiesRemainingText.text = "Zombies Remaining: " + DefenderManager.Instance.NumZombies.ToString();
        if (announcementCoroutine == null && announcements.Count > 0)
        {
            announcementCoroutine = StartCoroutine(ShowAnnouncement(announcements.Dequeue()));
        }
        if (waveNum > waves && leftZombieSpawner.FinishedSpawning && rightZombieSpawner.FinishedSpawning && DefenderManager.Instance.NumZombies == 0)
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
            //Debug.Log("Spawning wave...");
            AddAnnouncement("New Wave Spawning");
            leftZombieSpawner.SpawnWave();
            rightZombieSpawner.SpawnWave();
            if (waveNum % Player.Instance.Boosts.BoostWaveInterval == 0)
            {
                UIManager.Instance.SwitchState(UIManager.UIState.BOOSTS);
            }
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
        UIManager.Instance.DisableAllUI();
        announcements.Clear();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (win)
        {
            resultText.text = "You Win";
            resultText.color = Color.green;
            StartCoroutine(GameOverAnnouncement("Last Zombie Slain"));
            StartCoroutine(GameWon());
        }
        else
        {
            resultText.text = "You Lose";
            resultText.color = Color.red;
            StartCoroutine(GameOverAnnouncement("Beacon Destroyed"));
            StartCoroutine(GameLost());
        }
    }

    IEnumerator GameOverAnnouncement(string announcement)
    {
        announcementText.text = announcement;
        announcementText.alpha = 0;
        float t = 0;
        while (t < 0.5f)
        {
            t += Time.unscaledDeltaTime;
            announcementText.alpha = Mathf.Lerp(0, 1, t / 0.5f);
            yield return null;
        }
        announcementText.alpha = 1;
        yield return new WaitForSecondsRealtime(announcementTime);
        t = 0;
        while (t < 0.5f)
        {
            t += Time.unscaledDeltaTime;
            announcementText.alpha = Mathf.Lerp(1, 0, t / 0.5f);
            yield return null;
        }
    }

    IEnumerator GameWon()
    {
        Time.timeScale = 0.25f;
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(ShowGameOverUI());
        Time.timeScale = 0.0f;
        yield return null;
    }

    IEnumerator GameLost()
    {
        Time.timeScale = 0.0f;
        if (Player.Instance.Health.IsAlive)
        {
            gameOverCamera.transform.position = Player.Instance.Camera.transform.position;
            gameOverCamera.transform.rotation = Player.Instance.Camera.transform.rotation;
            Player.Instance.Camera.gameObject.SetActive(false);
        }
        else
        {
            gameOverCamera.transform.position = Respawn.Instance.Camera.transform.position;
            gameOverCamera.transform.rotation = Respawn.Instance.Camera.transform.rotation;
            Respawn.Instance.Camera.gameObject.SetActive(false);
        }
        gameOverCamera.SetActive(true);
        Vector3 startPos = gameOverCamera.transform.position;
        Quaternion startRot = gameOverCamera.transform.rotation;
        float t = 0;
        while (t < 5f)
        {
            t += Time.unscaledDeltaTime;
            gameOverCamera.transform.position = Vector3.Lerp(startPos, gameOverPosition.position, t / 5f);
            gameOverCamera.transform.rotation = Quaternion.Lerp(startRot, gameOverPosition.rotation, t / 5f);
        }
        gameOverCamera.transform.position = gameOverPosition.position;
        gameOverCamera.transform.rotation = gameOverPosition.rotation;
        beacon.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(ShowGameOverUI());
        yield return null;
    }

    IEnumerator ShowGameOverUI()
    {
        exitButton.SetActive(false);
        gameOverUI.enabled = true;
        Color color = gameOverPanel.color;
        color.a = 0;
        gameOverPanel.color = color;
        resultText.alpha = 0;
        gameOverText.alpha = 0;
        float t = 0;
        while (t < 3f)
        {
            t += Time.unscaledDeltaTime;
            resultText.alpha = Mathf.Lerp(0, 1, t / 3f);
            gameOverText.alpha = Mathf.Lerp(0, 1, t / 3f);
            color.a = t / 3f * (180.0f / 255.0f);
            gameOverPanel.color = color;
            yield return null;
        }
        resultText.alpha = 1;
        gameOverText.alpha = 1;
        color.a = 180.0f / 255.0f;
        gameOverPanel.color = color;
        exitButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return null;
    }


}
