/// <summary>
/// This script controls the zombie spawning for a zombie spawner.
/// </summary>

using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] constructTargets;
    [SerializeField] private GameObject zombie;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            Spawn();
        }
    }

    /// <summary>
    /// Spawns a zombie and initializes its targets.
    /// </summary>
    void Spawn()
    {
        GameObject newZombie = Instantiate(zombie);
        newZombie.GetComponent<ZombieMovement>().SetTargets(player, constructTargets); // initialize zombie's targets
    }
}
