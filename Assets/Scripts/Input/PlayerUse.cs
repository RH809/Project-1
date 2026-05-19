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
    [SerializeField] private Canvas swordHUD;
    //[SerializeField] private GameObject gun;
    //[SerializeField] private GameObject trap;
    //[SerializeField] private GameObject turret;

    private PlayerControls playerControls;
    private PlayerCamera playerCamera;

    [SerializeField] private float swordCooldown = 0.5f;
    private float cooldownTime = 0.0f;

    void Awake()
    {
        playerControls = new PlayerControls();
        playerCamera = GetComponent<PlayerCamera>();
    }
    void Start()
    {
        equippedItem = item.SWORD;
        UpdateActiveItem();
    }

    void OnEnable()
    {
        playerControls.Player.Use.performed += OnUsePerformed;
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Player.Use.performed -= OnUsePerformed;
        playerControls.Disable();
    }

    private void OnUsePerformed(InputAction.CallbackContext ctx) {
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
        swordHUD.enabled = (equippedItem == item.SWORD);
    }

    void Use()
    {
        if (cooldownTime <= 0.0f)
        {
            switch (equippedItem)
            {
                case item.SWORD:
                    cooldownTime = swordCooldown;
                    float rand = Random.Range(0.0f, 1.0f);
                    if (rand <= 1 / 3.0f)
                    {
                        playerAnimator.SetTrigger("Sword Swing 1");
                    }
                    else if (rand <= 2 / 3.0f)
                    {
                        playerAnimator.SetTrigger("Sword Swing 2");
                    }
                    else
                    {
                        playerAnimator.SetTrigger("Sword Swing 3");
                    }
                    playerCamera.LockCamera();
                    
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("On cooldown");
        }
    }
}
