/// <summary>
/// This script creates a static instance reference to the player.
/// </summary>
using UnityEngine;

public class Player : Singleton<Player>
{

    [HideInInspector] public Camera Camera;
    [HideInInspector] public Health Health;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerStamina Stamina;
    [HideInInspector] public PlayerInventory Inventory;
    [HideInInspector] public PlayerBank Bank;
    [HideInInspector] public PlayerPowerUp PowerUp;
    [HideInInspector] public PlayerBoosts Boosts;
    [HideInInspector] public PlayerInputManager InputManager;
    

    private PlayerCamera playerCamera;
    private SwordHitbox swordHitbox;

    protected override void Awake()
    {
        base.Awake();

        Camera = GetComponentInChildren<Camera>();
        Health = GetComponent<Health>();
        Movement = GetComponent<PlayerMovement>();
        Stamina = GetComponent<PlayerStamina>();
        Inventory = GetComponent<PlayerInventory>();
        Bank = GetComponent<PlayerBank>();
        PowerUp = GetComponent<PlayerPowerUp>();
        Boosts = GetComponent<PlayerBoosts>();
        InputManager = GetComponent<PlayerInputManager>();

        playerCamera = GetComponent<PlayerCamera>();
        swordHitbox = GetComponent<SwordHitbox>();
    }

    void OnEnable()
    {
        Health.OnDie += OnPlayerDeath;
    }

    private void OnDisable()
    {
        Health.OnDie -= OnPlayerDeath;
    }

    private void Update()
    {
        if (GameManager.Instance.DEBUG && Input.GetKeyDown("y"))
        {
            if (this.Health.IsAlive)
            {
                this.Health.TakeDamage(this.Health.MaxHealth, gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        // handle ordering of LateUpdate method calls
        playerCamera.UpdateArmRotation(); // arm rotation update should come before sword hitbox detection
        swordHitbox.HitDetection();
    }

    void OnPlayerDeath(HealthContext healthContext)
    {
        if (healthContext.target == gameObject)
        {
            GameManager.Instance.AddAnnouncement("You Have Been Slain");
        }
    }
}
