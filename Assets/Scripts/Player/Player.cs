/// <summary>
/// This script creates a static instance reference to the player.
/// </summary>
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [HideInInspector] public Camera Camera;
    [HideInInspector] public Health Health;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerInventory Inventory;
    [HideInInspector] public PlayerInputManager InputManager;

    private PlayerCamera playerCamera;
    private SwordHitbox swordHitbox;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Camera = GetComponentInChildren<Camera>();
        Health = GetComponent<Health>();
        Movement = GetComponent<PlayerMovement>();
        Inventory = GetComponent<PlayerInventory>();
        InputManager = GetComponent<PlayerInputManager>();

        playerCamera = GetComponent<PlayerCamera>();
        swordHitbox = GetComponent<SwordHitbox>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("y"))
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
}
