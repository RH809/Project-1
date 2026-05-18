using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUse : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

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

    private PlayerControls playerControls;

    [SerializeField] private float swordCooldown = 0.5f;
    private float cooldownTime = 0.0f;

    void Awake()
    {
        playerControls = new PlayerControls();
    }
    void Start()
    {
        equippedItem = item.SWORD;
    }

    void OnEnable()
    {
        playerControls.Player.Use.performed += OnUsePerformed;
        playerControls.Enable();
        Debug.Log(playerControls + " enabled");
    }

    void OnDisable()
    {
        playerControls.Player.Use.performed -= OnUsePerformed;
        playerControls.Disable();
    }

    private void OnUsePerformed(InputAction.CallbackContext ctx) {
        Debug.Log("Using...");
        Use();
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
                    float rand = Random.Range(0.0f, 1.0f);
                    if (rand <= 1 / 3.0f)
                    {
                        Debug.Log("1");
                        playerAnimator.SetTrigger("Sword Swing 1");
                    }
                    else if (rand <= 2 / 3.0f)
                    {
                        Debug.Log("2");
                        playerAnimator.SetTrigger("Sword Swing 2");
                    }
                    else
                    {
                        Debug.Log("3");
                        playerAnimator.SetTrigger("Sword Swing 2");
                    }
                    
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
