/// <summary>
/// This script manages all of the defenders on the map.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    public static DefenderManager Instance;

    [SerializeField] private GameObject[] defenders;
    private LinkedList<GameObject> zombies;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        zombies = new LinkedList<GameObject>();
    }

    void OnEnable()
    {
        Health.OnDie += RemoveZombie;
    }

    void OnDisable()
    {
        Health.OnDie -= RemoveZombie;
    }

    void RemoveZombie(HealthContext healthContext)
    {
        if (healthContext.target.GetComponent<Zombie>() != null)
        {
            zombies.Remove(healthContext.target);
        }
    }

    public void AddToZombieList(GameObject zombie)
    {
        //Debug.Log("Adding zombie to list...");
        zombies.AddLast(zombie);
    }

    public GameObject GetDefenderTarget(Vector3 pos, float range)
    {
        GameObject closestTank = null;
        float closestTankDist = float.MaxValue;
        GameObject closestRegular = null;
        float closestRegularDist = float.MaxValue;
        GameObject closestMini = null;
        float closestMiniDist = float.MaxValue;
        foreach (GameObject z in zombies)
        {
            float dist = Mathf.Sqrt(Mathf.Pow(pos.x - z.transform.position.x, 2) + Mathf.Pow(pos.z - z.transform.position.z, 2));
            //Debug.Log($"Zombie dist: {dist} {pos} {z.transform.position}");
            if (dist <= range)
            {
                switch (z.GetComponent<Zombie>().Type)
                {
                    case Zombie.ZombieType.TANK:
                        if (closestTankDist > dist)
                        {
                            closestTankDist = dist;
                            closestTank = z;
                        }
                        break;
                    case Zombie.ZombieType.REGULAR:
                        if (closestRegularDist > dist)
                        {
                            closestRegularDist = dist;
                            closestRegular = z;
                        }
                        break;
                    case Zombie.ZombieType.MINI:
                        if (closestMiniDist > dist)
                        {
                            closestMiniDist = dist;
                            closestMini = z;
                        }
                        break;
                }
            }
        }
        if (closestTank != null)
        {
            return closestTank;
        }
        else if (closestRegular != null)
        {
            return closestRegular;
        }
        else
        {
            return closestMini;
        }
    }
}
