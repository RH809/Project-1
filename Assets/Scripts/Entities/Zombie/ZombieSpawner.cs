/// <summary>
/// This script controls the zombie spawning for a zombie spawner.
/// </summary>

using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] constructTargets;
    [SerializeField] private GameObject regularZombie;
    [SerializeField] private GameObject miniZombie;
    [SerializeField] private GameObject tankZombie;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            SpawnRegularZombie();
        }
        if (Input.GetKeyDown("k"))
        {
            SpawnMiniZombie();
        }
        if (Input.GetKeyDown("j"))
        {
            SpawnTankZombie();
        }
    }

    /// <summary>
    /// Spawns a random zombie.
    /// </summary>
    void Spawn()
    {
        
    }

    /// <summary>
    /// Spawns a regular zombie.
    /// </summary>
    void SpawnRegularZombie()
    {
        GameObject newZombie = Instantiate(regularZombie, transform.position, transform.rotation);
        newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
    }

    /// <summary>
    /// Spawns a mini zombie.
    /// </summary>
    void SpawnMiniZombie()
    {
        GameObject newZombie = Instantiate(miniZombie, transform.position, transform.rotation);
        newZombie.GetComponent<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
    }

    /// <summary>
    /// Spawns a tank zombie.
    /// </summary>
    void SpawnTankZombie()
    {
        GameObject newZombie = Instantiate(tankZombie, transform.position, transform.rotation);
        newZombie.GetComponentInChildren<ZombieMovement>().SetTargets(constructTargets); // initialize zombie's targets
    }
}
