/// <summary>
/// This script manages the player's boosts.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoosts : MonoBehaviour
{
    [SerializeField] private HemorrhageBoost hemorrhageBoostBase;
    [SerializeField] private PiercingBoost piercingBoostBase;

    [HideInInspector] public HemorrhageBoost hemorrhageBoost;
    [HideInInspector] public PiercingBoost piercingBoost;

    private List<Boost> boosts;

    void Awake()
    {
        hemorrhageBoost = Instantiate(hemorrhageBoostBase);
        piercingBoost = Instantiate(piercingBoostBase);

        boosts.Add(hemorrhageBoost);
        boosts.Add(piercingBoost);
    }
}
