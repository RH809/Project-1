/// <summary>
/// This script manages the player's boosts.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoosts : MonoBehaviour
{
    [SerializeField] private int boostWaveInterval;
    public int BoostWaveInterval { get => boostWaveInterval; }

    [SerializeField] private HemorrhageBoost hemorrhageBoostBase;
    [SerializeField] private PiercingBoost piercingBoostBase;
    [SerializeField] private PaydayBoost paydayBoostBase;
    [SerializeField] private CollectorBoost collectorBoostBase;
    [SerializeField] private AcceleratedAscensionBoost acceleratedAscensionBoostBase;
    [SerializeField] private TechnologicalAdvancementBoost technologicalAdvancementBoostBase;
    [SerializeField] private VampiricBladeBoost vampiricBladeBoostBase;
    [SerializeField] private StunGunBoost stunGunBoostBase;

    [HideInInspector] public HemorrhageBoost Hemorrhage;
    [HideInInspector] public PiercingBoost Piercing;
    [HideInInspector] public PaydayBoost Payday;
    [HideInInspector] public CollectorBoost Collector;
    [HideInInspector] public AcceleratedAscensionBoost AcceleratedAscension;
    [HideInInspector] public TechnologicalAdvancementBoost TechnologicalAdvancement;
    [HideInInspector] public VampiricBladeBoost VampiricBlade;
    [HideInInspector] public StunGunBoost StunGun;

    private List<Boost> boosts;
    private List<Boost> selectedBoosts;

    void Awake()
    {
        Hemorrhage = Instantiate(hemorrhageBoostBase);
        Piercing = Instantiate(piercingBoostBase);
        Payday = Instantiate(paydayBoostBase);
        Collector = Instantiate(collectorBoostBase);
        AcceleratedAscension = Instantiate(acceleratedAscensionBoostBase);
        TechnologicalAdvancement = Instantiate(technologicalAdvancementBoostBase);
        VampiricBlade = Instantiate(vampiricBladeBoostBase);
        StunGun = Instantiate(stunGunBoostBase);

        boosts = new List<Boost>();
        boosts.Add(Hemorrhage);
        boosts.Add(Piercing);
        boosts.Add(Payday);
        boosts.Add(Collector);
        boosts.Add(AcceleratedAscension);
        boosts.Add(TechnologicalAdvancement);
        boosts.Add(VampiricBlade);
        boosts.Add(StunGun);

        selectedBoosts = new List<Boost>();

        if (GameManager.Instance.DEBUG)
        {
            //VampiricBlade.Select();
        }
    }

    public Boost[] GetBoosts()
    {
        int[] indices = { -1, -1, -1 };
        for (int i = 0; i < 3; i++)
        {
            int index = -1;
            do
            {
                index = Random.Range(0, boosts.Count);
            } while (indices[0] == index || indices[1] == index || boosts[index].IsMaxedOut);
            indices[i] = index;
        }
        return new Boost[] { boosts[indices[0]], boosts[indices[1]], boosts[indices[2]] };
    }

    public void SelectNewBoost(Boost boost)
    {
        if (boost.IsPermanent) selectedBoosts.Add(boost);
    }

    public string GetBoostList()
    {
        if (selectedBoosts.Count == 0) return "None";
        string list = "";
        foreach (Boost boost in selectedBoosts)
        {
            list += boost.CurrentName + "\n";
        }
        return list;
    }
}
