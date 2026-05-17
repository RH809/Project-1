using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    public enum item {
        SWORD = 0,
        GUN = 1,
        TRAP = 2,
        TURRET = 3
    };

    private item equippedItem;

    [SerializeField] private GameObject sword;
    //[SerializeField] private GameObject gun;
    //[SerializeField] private GameObject trap;
    //[SerializeField] private GameObject turret;

    private PlayerInput playerInput;

    [SerializeField] private float swordCooldown = 0.5f;
    private float cooldownTime = 0.0f;

    void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.Player.Use.performed += ctx =>
        {
            Use();
        };
    }
    void Start()
    {
        equippedItem = item.SWORD;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActiveItem();
        if (cooldownTime > 0.0f) {
            cooldownTime -= Time.deltaTime;
            cooldownTime = Mathf.Max(cooldownTime, 0.0f);
        }
    }

    void UpdateActiveItem()
    {
        sword.SetActive(equippedItem == item.SWORD);
    }

    void Use()
    {
        if (cooldownTime <= 0.0f)
        {
            switch (equippedItem)
            {
                case item.SWORD:
                    Debug.Log("Using sword");
                    cooldownTime = swordCooldown;
                    break;
                default:
                    Debug.Log("Default");
                    break;
            }
        }
        else
        {
            Debug.Log("On cooldown");
        }
    }
}
