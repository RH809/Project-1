/// <summary>
/// This script controls the zombie spawning for a zombie spawner and the object pooling for
/// instantiations regarding zombies.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] constructTargets;
    [SerializeField] private Zombie regularZombie;
    [SerializeField] private Zombie miniZombie;
    [SerializeField] private Zombie tankZombie;

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

    private ObjectPool<Zombie> regularZombiePool;
    private ObjectPool<Zombie> miniZombiePool;
    private ObjectPool<Zombie> tankZombiePool;

    private ObjectPool<TankZombieAttack> tankZombieAttackPool;


    void Start()
    {
        zombies = new List<Zombie.ZombieType>();
        numRegularZombies = regularZombieBaseSpawns;
        numMiniZombies = miniZombieBaseSpawns;
        numTankZombies = tankZombieBaseSpawns;

        regularZombiePool = new ObjectPool<Zombie>(
            CreateRegularZombie,
            OnTakeRegularZombie,
            OnReturnRegularZombie,
            OnDestroyRegularZombie,
            true,
            20,
            100
        );

        miniZombiePool = new ObjectPool<Zombie>(
            CreateMiniZombie,
            OnTakeMiniZombie,
            OnReturnMiniZombie,
            OnDestroyMiniZombie,
            true,
            20,
            100
        );

        tankZombiePool = new ObjectPool<Zombie>(
            CreateTankZombie,
            OnTakeTankZombie,
            OnReturnTankZombie,
            OnDestroyTankZombie,
            true,
            10,
            50
        );
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
        if (GameManager.Instance.WaveNum % tankZombieSpawnIncreaseInterval == 0)
        {
            for (int i = 0; i < numTankZombies + disruptorDeaths * tankZombieDisruptorIncrease; i++)
            {
                zombies.Add(Zombie.ZombieType.TANK);
            }
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
        //Zombie newZombie = Instantiate(regularZombie, transform.position, transform.rotation);
        //newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
        //DefenderManager.Instance.AddToZombieList(newZombie.gameObject);
        Zombie newZombie = regularZombiePool.Get();
        //newZombie.transform.SetPositionAndRotation(transform.position, transform.rotation);
        //newZombie.Spawn(transform.position, transform.rotation);
        DefenderManager.Instance.AddToZombieList(newZombie.gameObject);
    }

    /// <summary>
    /// Spawns a mini zombie.
    /// </summary>
    void SpawnMiniZombie()
    {
        //Zombie newZombie = Instantiate(miniZombie, transform.position, transform.rotation);
        //newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
        //DefenderManager.Instance.AddToZombieList(newZombie.gameObject);
        Zombie newZombie = miniZombiePool.Get();
        //newZombie.transform.SetPositionAndRotation(transform.position, transform.rotation);
        //newZombie.Spawn(transform.position, transform.rotation);
        DefenderManager.Instance.AddToZombieList(newZombie.gameObject);
    }

    /// <summary>
    /// Spawns a tank zombie.
    /// </summary>
    void SpawnTankZombie()
    {
        //Zombie newZombie = Instantiate(tankZombie, transform.position, transform.rotation);
        //newZombie.GetComponentInChildren<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
        //DefenderManager.Instance.AddToZombieList(newZombie.gameObject);
        Zombie newZombie = tankZombiePool.Get();
        //newZombie.transform.SetPositionAndRotation(transform.position, transform.rotation);
        //newZombie.Spawn(transform.position, transform.rotation);
        DefenderManager.Instance.AddToZombieList(newZombie.gameObject);
    }

    private Zombie CreateRegularZombie()
    {
        Zombie newZombie = Instantiate(regularZombie);
        newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets);
        newZombie.SetPool(regularZombiePool);
        return newZombie;
    }

    private void OnTakeRegularZombie(Zombie zombie)
    {
        zombie.Spawn(transform.position, transform.rotation);
        zombie.gameObject.SetActive(true);
        
    }

    private void OnReturnRegularZombie(Zombie zombie)
    {
        zombie.gameObject.SetActive(false);
    }

    private void OnDestroyRegularZombie(Zombie zombie)
    {
        Destroy(zombie.gameObject);
    }

    private Zombie CreateMiniZombie()
    {
        Zombie newZombie = Instantiate(miniZombie);
        newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets);
        newZombie.SetPool(miniZombiePool);
        return newZombie;
    }

    private void OnTakeMiniZombie(Zombie zombie)
    {
        zombie.Spawn(transform.position, transform.rotation);
        zombie.gameObject.SetActive(true);
    }

    private void OnReturnMiniZombie(Zombie zombie)
    {
        zombie.gameObject.SetActive(false);
    }

    private void OnDestroyMiniZombie(Zombie zombie)
    {
        Destroy(zombie.gameObject);
    }

    private Zombie CreateTankZombie()
    {
        Zombie newZombie = Instantiate(tankZombie);
        newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets);
        newZombie.SetPool(tankZombiePool);
        return newZombie;
    }

    private void OnTakeTankZombie(Zombie zombie)
    {
        zombie.Spawn(transform.position, transform.rotation);
        zombie.gameObject.SetActive(true);
    }

    private void OnReturnTankZombie(Zombie zombie)
    {
        zombie.gameObject.SetActive(false);
    }

    private void OnDestroyTankZombie(Zombie zombie)
    {
        Destroy(zombie.gameObject);
    }

}
