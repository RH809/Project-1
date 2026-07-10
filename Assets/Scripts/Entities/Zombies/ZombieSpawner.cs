/// <summary>
/// This script controls the zombie spawning for a zombie spawner.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] constructTargets;
    [SerializeField] private GameObject regularZombie;
    [SerializeField] private GameObject miniZombie;
    [SerializeField] private GameObject tankZombie;

    [SerializeField] private Construct disruptor;

    private int disruptorDeaths = 0; // each disruptor death increases spawns
    [SerializeField] private float spawnInterval = 0.5f;

    [SerializeField] private int regularZombieBaseSpawns;
    [SerializeField] private int miniZombieBaseSpawns;
    [SerializeField] private int tankZombieBaseSpawns;

    [SerializeField] private int regularZombieSpawnIncrease;
    [SerializeField] private int regularZombieSpawnIncreaseInterval;
    [SerializeField] private int miniZombieSpawnIncrease;
    [SerializeField] private int miniZombieSpawnIncreaseInterval;
    [SerializeField] private int tankZombieSpawnIncrease;
    [SerializeField] private int tankZombieSpawnIncreaseInterval;

    [SerializeField] private int regularZombieDisruptorIncrease;
    [SerializeField] private int miniZombieDisruptorIncrease;
    [SerializeField] private int tankZombieDisruptorIncrease;

    private List<Zombie.ZombieType> zombies;
    private int numRegularZombies;
    private int numMiniZombies;
    private int numTankZombies;

    private bool finishedSpawning;
    public bool FinishedSpawning { get => finishedSpawning; }

    void Start()
    {
        zombies = new List<Zombie.ZombieType>();
        numRegularZombies = regularZombieBaseSpawns;
        numMiniZombies = miniZombieBaseSpawns;
        numTankZombies = tankZombieBaseSpawns;
    }

    void Update()
    {
        if (GameManager.Instance.DEBUG && Input.GetKeyDown("l"))
        {
            SpawnRegularZombie();
        }
        if (GameManager.Instance.DEBUG && Input.GetKeyDown("k"))
        {
            SpawnMiniZombie();
        }
        if (GameManager.Instance.DEBUG && Input.GetKeyDown("j"))
        {
            SpawnTankZombie();
        }
    }

    void OnEnable()
    {
        Health.OnDie += OnDisruptorDie;
    }

    void OnDisable()
    {
        Health.OnDie -= OnDisruptorDie;
    }

    void OnDisruptorDie(HealthContext healthContext)
    {
        if (healthContext.target.Equals(disruptor))
        {
            disruptorDeaths++;
        }
    }

    /// <summary>
    /// Creates list of zombies that will be spawned this wave.
    /// </summary>
    public void WaveSetup()
    {
        // Update spawn numbers base on increase intervals
        if (GameManager.Instance.WaveNum % regularZombieSpawnIncreaseInterval == 0)
        {
            numRegularZombies += regularZombieSpawnIncrease;
        }
        if (GameManager.Instance.WaveNum % miniZombieSpawnIncreaseInterval == 0)
        {
            numMiniZombies += miniZombieSpawnIncrease;
        }
        if (GameManager.Instance.WaveNum % tankZombieSpawnIncreaseInterval == 0)
        {
            numTankZombies += tankZombieSpawnIncrease;
        }
        for (int i = 0; i < numRegularZombies + disruptorDeaths * regularZombieDisruptorIncrease; i++)
        {
            zombies.Add(Zombie.ZombieType.REGULAR);
        }
        for (int i = 0; i < numMiniZombies + disruptorDeaths * miniZombieDisruptorIncrease; i++)
        {
            zombies.Add(Zombie.ZombieType.MINI);
        }
        for (int i = 0; i < numTankZombies + disruptorDeaths * tankZombieDisruptorIncrease; i++)
        {
            zombies.Add(Zombie.ZombieType.TANK);
        }
    }

    public void SpawnWave()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    /// <summary>
    /// Coroutine that spawns all zombies in the list at a set interval.
    /// </summary>
    IEnumerator SpawnWaveRoutine()
    {
        finishedSpawning = false;
        while (zombies.Count > 0)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
        finishedSpawning = true;
        yield return null;
    }

    /// <summary>
    /// Spawns a random zombie from the list.
    /// </summary>
    void Spawn()
    {
        int index = Random.Range(0, zombies.Count);
        switch (zombies[index])
        {
            case Zombie.ZombieType.REGULAR:
                SpawnRegularZombie();
                break;
            case Zombie.ZombieType.MINI:
                SpawnMiniZombie();
                break;
            case Zombie.ZombieType.TANK:
                SpawnTankZombie();
                break;
        }
        zombies.RemoveAt(index);
    }

    /// <summary>
    /// Spawns a regular zombie.
    /// </summary>
    void SpawnRegularZombie()
    {
        GameObject newZombie = Instantiate(regularZombie, transform.position, transform.rotation);
        newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
        DefenderManager.Instance.AddToZombieList(newZombie);
    }

    /// <summary>
    /// Spawns a mini zombie.
    /// </summary>
    void SpawnMiniZombie()
    {
        GameObject newZombie = Instantiate(miniZombie, transform.position, transform.rotation);
        newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
        DefenderManager.Instance.AddToZombieList(newZombie);
    }

    /// <summary>
    /// Spawns a tank zombie.
    /// </summary>
    void SpawnTankZombie()
    {
        GameObject newZombie = Instantiate(tankZombie, transform.position, transform.rotation);
        newZombie.GetComponentInChildren<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
        DefenderManager.Instance.AddToZombieList(newZombie);
    }
}
