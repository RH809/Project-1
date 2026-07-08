using UnityEngine;

public class PlayerBoosts : MonoBehaviour
{
    [SerializeField] private HemorrhageBoost hemorrhageBoostBase;

    [HideInInspector] public HemorrhageBoost hemorrhageBoost;

    void Awake()
    {
        hemorrhageBoost = Instantiate(hemorrhageBoostBase);
    }
}
