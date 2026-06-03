/// <summary>
/// This script manages the player's held item and handles using the items.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    public enum item {
        SWORD = 0,
        GUN = 1,
        TRAP = 2,
        REPAIR_TOOL = 3,
        EMPTY = 4
    };

    private item equippedItem;
    private Queue<item> equipQueue;
    private ArrayList<item> slotItems;
    private int repairToolCount = 0;
    private int trapCount = 0;
    private int equippedIndex;

    [SerializeField] private Canvas aimHUD;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject repairTool;
    //[SerializeField] private GameObject trap;
    //[SerializeField] private GameObject turret;

    private PlayerControls playerControls;
    private PlayerCamera playerCamera;
    private SwordHitbox swordHitbox;
    private Shoot shoot;

    [SerializeField] private float swordCooldown = 0.5f;
    [SerializeField] private float gunCooldown = 1f;
    private float cooldownTime = 0.0f;

    void Awake()
    {
        playerControls = new PlayerControls();
        playerCamera = GetComponent<PlayerCamera>();
        swordHitbox = GetComponent<SwordHitbox>();
        shoot = GetComponent<Shoot>();
    }
    void Start()
    {
        slotItems = new LinkedList<item>();
        slotItems.AddLast(item.SWORD);
        slotItems.AddLast(item.GUN);
        equipQueue = new Queue<item>();
        equippedItem = item.SWORD;
        UpdateActiveItem();
    }

    void OnEnable()
    {
        playerControls.Player.Use.performed += OnUsePerformed;
        playerControls.Player.SelectItem.performed += OnSelectItemPerformed;
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Player.Use.performed -= OnUsePerformed;
        playerControls.Player.SelectItem.performed -= OnSelectItemPerformed;
        playerControls.Disable();
    }

    private void OnUsePerformed(InputAction.CallbackContext ctx) {
        Use();
    }

    private void OnSelectItemPerformed(InputAction.CallbackContext ctx) {
        int value = (int)ctx.ReadValue<float>();
        Debug.Log(value + " " + (int)equippedItem);
        if (value >= slotItems.Count || slotItems.== equippedItem)
        {
            Debug.Log($"Trying to equip same item. {slotItems[value]} {equippedItem}");
            return; // don't do anything if they selected the already-equipped item
        }
        switch (value)
        {
            case 0:
                //playerAnimator.SetTrigger("Equip Sword");
                //equipQueue.Enqueue(item.SWORD); // add to queue
                //equippedItem = item.SWORD;
                //break;
            case 1:
                //playerAnimator.SetTrigger("Equip Gun");
                //equipQueue.Enqueue(item.GUN); // add to queue
                //equippedItem = item.GUN;
                equipQueue.Enqueue(slotItems[value]);
                break;
            case 2:
            case 3:
                if (slotItems[value] != item.EMPTY && count[value - 2] > 0) {
                    equipQueue.Enqueue(slotItems[value]);
                }
                break;
        }
        
    }

    void Update()
    {
        UpdateActiveItem();
        if (cooldownTime > 0.0f) { // reduce cooldown
            cooldownTime -= Time.deltaTime;
            cooldownTime = Mathf.Max(cooldownTime, 0.0f);
        }
        if (equippedItem == item.REPAIR_TOOL)
        {
            // Raycast for construct to be repaired
        }
    }

    /// <summary>
    /// Updates the equipped item and the active state of the items.
    /// </summary>
    void UpdateActiveItem()
    {
        if (!(equippedItem == item.SWORD && IsInSwingAnimation()) &&
            !(equippedItem == item.GUN && IsInShootAnimation()))
        { // update equipped item if not in use animation
            item prev = equippedItem;
            while (equipQueue.Count > 0)
            { // choose the last selected item in the queue
                equippedItem = equipQueue.Dequeue();
            }
            if (prev != equippedItem)
            { // trigger animation if equipped new item
                switch (equippedItem)
                {
                    case item.SWORD:
                        Debug.Log("Equip sword animation");
                        playerAnimator.SetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Repair Tool");
                        break;
                    case item.GUN:
                        Debug.Log("Equip gun animation");
                        playerAnimator.SetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Repair Tool");
                        break;
                    case item.TRAP:
                        break;
                    case item.REPAIR_TOOL:
                        Debug.Log("Equip repair tool animation");
                        playerAnimator.SetTrigger("Equip Repair Tool");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Gun");
                        break;
                }
            }
            
        }
        sword.SetActive(equippedItem == item.SWORD);
        gun.SetActive(equippedItem == item.GUN);
        repairTool.SetActive(equippedItem == item.REPAIR_TOOL);
        aimHUD.enabled = (equippedItem == item.SWORD || equippedItem == item.GUN || equippedItem == item.REPAIR_TOOL);
    }

    /// <summary>
    /// Checks if the player is currently in the sword swing animation
    /// </summary>
    /// <returns>Whether the player is in the sword swing animation</returns>
    private bool IsInSwingAnimation()
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Sword Swing 1") || playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Sword Swing 2") ||
            playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Sword Swing 3") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Sword Swing 1") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Sword Swing 2") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Sword Swing 3") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Swing 1 -> Sword Idle") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Swing 2 -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Swing 3 -> Sword Idle") || playerAnimator.GetBool("Sword Swing 1") || playerAnimator.GetBool("Sword Swing 2") || playerAnimator.GetBool("Sword Swing 3");
    }

    /// <summary>
    /// Checks if the player is currently in the shoot animation
    /// </summary>
    /// <returns>Whether the player is in the shoot animation</returns>
    private bool IsInShootAnimation()
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Gun Shoot") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Gun Shoot") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Shoot -> Gun Idle") || playerAnimator.GetBool("Shoot");
    }

    private bool IsInSwitchItemAnimation()
    {
        return playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Gun Idle") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Sword Idle");
    }

    /// <summary>
    /// Uses the currently held item
    /// </summary>
    void Use()
    {
        if (IsInSwitchItemAnimation()) return;
        if (cooldownTime <= 0.0f) // must be off cooldown
        {
            switch (equippedItem)
            {
                case item.SWORD:
                    if (swordHitbox.isSwinging() || IsInSwingAnimation()) { // don't use if already in swinging animation
                        break;
                    }
                    cooldownTime = swordCooldown;
                    // Randomly choose between the 3 possible animations
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
                    playerCamera.LockCamera(); // Lock player camera during the swing
                    
                    break;
                case item.GUN:
                    if (shoot.isShooting() || IsInShootAnimation()) { // don't use if already in shooting animation
                        break;
                    }
                    cooldownTime = gunCooldown;
                    playerAnimator.SetTrigger("Shoot");
                    break;
            }
        }
        else
        {
            Debug.Log("On cooldown");
        }
    }
}
