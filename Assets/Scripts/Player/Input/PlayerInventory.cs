/// <summary>
/// This script manages the player's held item and handles using the items.
/// </summary>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    public enum Item {
        SWORD = 0,
        GUN = 1,
        GRENADE = 2,
        REPAIR_TOOL = 3,
        HEALTH_POTION = 4
    };

    private Item equippedItem;
    public Item EquippedItem { get => equippedItem; }
    private Queue<Item> equipQueue;
    private List<Item> slotItems;
    private int repairToolCount = 0;
    private int grenadeCount = 0;
    private int equippedIndex;

    [SerializeField] private Canvas aimHUD;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject repairTool;
    [SerializeField] private GameObject grenade;
    //[SerializeField] private GameObject turret;

    //private PlayerControls playerControls;
    private PlayerCamera playerCamera;
    private SwordHitbox swordHitbox;
    private Shoot shoot;
    private GrenadeThrow grenadeThrow;

    [SerializeField] private float swordCooldown = 0.5f;
    [SerializeField] private float gunCooldown = 1f;
    [SerializeField] private float grenadeCooldown = 1f;
    private float cooldownTime = 0.0f;

    void Awake()
    {
        //playerControls = new PlayerControls();
        playerCamera = GetComponent<PlayerCamera>();
        swordHitbox = GetComponent<SwordHitbox>();
        shoot = GetComponent<Shoot>();
        grenadeThrow = GetComponent<GrenadeThrow>();
    }
    void Start()
    {
        slotItems = new List<Item>();
        slotItems.Add(Item.SWORD);
        slotItems.Add(Item.GUN);
        // ==== TESTING ======
        AddRepairTool();
        AddGrenade();
        // ===================
        equipQueue = new Queue<Item>();
        equippedItem = Item.SWORD;
        UpdateActiveItem();
    }

    void OnEnable()
    {
        Player.Instance.InputManager.Controls.Player.Use.performed += OnUsePerformed;
        Player.Instance.InputManager.Controls.Player.SelectItem.performed += OnSelectItemPerformed;
        Player.Instance.InputManager.Controls.Player.Scroll.performed += OnScrollPerformed;
        /*
        Player.Instance.Controls.Player.Use.performed += OnUsePerformed;
        Player.Instance.Controls.Player.SelectItem.performed += OnSelectItemPerformed;
        Player.Instance.Controls.Player.Scroll.performed += OnScrollPerformed;
        */
        /*
        playerControls.Player.Use.performed += OnUsePerformed;
        playerControls.Player.SelectItem.performed += OnSelectItemPerformed;
        playerControls.Player.Scroll.performed += OnScrollPerformed;
        playerControls.Enable();
        */
    }

    void OnDisable()
    {
        Player.Instance.InputManager.Controls.Player.Use.performed -= OnUsePerformed;
        Player.Instance.InputManager.Controls.Player.SelectItem.performed -= OnSelectItemPerformed;
        Player.Instance.InputManager.Controls.Player.Scroll.performed -= OnScrollPerformed;
        /*
        Player.Instance.Controls.Player.Use.performed -= OnUsePerformed;
        Player.Instance.Controls.Player.SelectItem.performed -= OnSelectItemPerformed;
        Player.Instance.Controls.Player.Scroll.performed -= OnScrollPerformed;
        */
        /*
        playerControls.Player.Use.performed -= OnUsePerformed;
        playerControls.Player.SelectItem.performed -= OnSelectItemPerformed;
        playerControls.Player.Scroll.performed -= OnScrollPerformed;
        playerControls.Disable();
        */
    }

    private void OnUsePerformed(InputAction.CallbackContext ctx) {
        Use();
    }

    private void OnSelectItemPerformed(InputAction.CallbackContext ctx) {
        int value = (int)ctx.ReadValue<float>();
        //Debug.Log(value + " " + (int)equippedItem);
        if (value >= slotItems.Count || slotItems[value] == equippedItem)
        {
            return; // don't do anything if they selected the already-equipped item or out of bounds
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
            case 2:
            case 3:
                equipQueue.Enqueue(slotItems[value]);
                break;
        }
    }

    private void OnScrollPerformed(InputAction.CallbackContext ctx)
    {
        float scroll = ctx.ReadValue<Vector2>().y;
        if (scroll == 0) return;
        int newIndex = equippedIndex + (scroll > 0 ? -1 : 1); // positive scroll = index goes down
        if (newIndex < 0)
        {
            newIndex = slotItems.Count - 1;
        }
        else if (newIndex >= slotItems.Count)
        {
            newIndex = 0;
        }
        equipQueue.Enqueue(slotItems[newIndex]);
    }

    void Update()
    {
        UpdateActiveItem();
        if (cooldownTime > 0.0f) { // reduce cooldown
            cooldownTime -= Time.deltaTime;
            cooldownTime = Mathf.Max(cooldownTime, 0.0f);
        }
    }

    /// <summary>
    /// Updates the equipped item and the active state of the items.
    /// </summary>
    void UpdateActiveItem()
    {
        if (!(equippedItem == Item.SWORD && IsInSwingAnimation()) &&
            !(equippedItem == Item.GUN && IsInShootAnimation()) &&
            !(equippedItem == Item.GRENADE && IsInThrowAnimation()))
        { // update equipped item if not in use animation
            Item prev = equippedItem;
            while (equipQueue.Count > 0)
            { // choose the last selected item in the queue
                equippedItem = equipQueue.Dequeue();
            }
            equippedIndex = slotItems.IndexOf(equippedItem);
            if (prev != equippedItem)
            { // trigger animation if equipped new item
                switch (equippedItem)
                {
                    case Item.SWORD:
                        //Debug.Log("Equip sword animation");
                        playerAnimator.SetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Grenade");
                        playerAnimator.ResetTrigger("Equip Repair Tool");
                        break;
                    case Item.GUN:
                        //Debug.Log("Equip gun animation");
                        playerAnimator.SetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Grenade");
                        playerAnimator.ResetTrigger("Equip Repair Tool");
                        break;
                    case Item.GRENADE:
                        //Debug.Log("Equip grenade animation");
                        playerAnimator.SetTrigger("Equip Grenade");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Repair Tool");
                        break;
                    case Item.REPAIR_TOOL:
                        //Debug.Log("Equip repair tool animation");
                        playerAnimator.SetTrigger("Equip Repair Tool");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Grenade");
                        break;
                }
            }
            
        }
        sword.SetActive(equippedItem == Item.SWORD);
        gun.SetActive(equippedItem == Item.GUN);
        repairTool.SetActive(equippedItem == Item.REPAIR_TOOL);
        grenade.SetActive(equippedItem == Item.GRENADE && !grenadeThrow.ThrowingGrenade);
        aimHUD.enabled = (equippedItem != Item.HEALTH_POTION);
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

    /// <summary>
    /// Checks if the player is currently in the grenade throw animation
    /// </summary>
    /// <returns>Whether the player is in the grenade throw animation</returns>
    private bool IsInThrowAnimation()
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Grenade Throw") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Idle -> Grenade Throw") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Throw -> Grenade Idle") || playerAnimator.GetBool("Grenade Throw");
    }

    /// <summary>
    /// Checks if the player is currently in an animation transition of switching between items
    /// </summary>
    /// <returns>Whether the player is in an animation transition for switching items</returns>
    private bool IsInSwitchItemAnimation()
    {
        return playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Gun Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Grenade Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Repair Tool Idle") ||

            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Grenade Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Repair Tool Idle") ||

            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Idle -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Idle -> Gun Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Idle -> Repair Tool Idle") ||

            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Repair Tool Idle -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Repair Tool Idle -> Gun Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Repair Tool Idle -> Grenade Idle");
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
                case Item.SWORD:
                    if (swordHitbox.isSwinging() || IsInSwingAnimation())
                    { // don't use if already in swinging animation
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
                case Item.GUN:
                    if (shoot.isShooting() || IsInShootAnimation())
                    { // don't use if already in shooting animation
                        break;
                    }
                    cooldownTime = gunCooldown;
                    playerAnimator.SetTrigger("Shoot");
                    break;
                case Item.GRENADE:
                    if (IsInThrowAnimation())
                    { // don't use if already in throwing animation
                        break;
                    }
                    cooldownTime = grenadeCooldown;
                    playerAnimator.SetTrigger("Grenade Throw");
                    break;
            }
        }
        else
        {
            Debug.Log("On cooldown");
        }
    }

    public void AddRepairTool()
    {
        repairToolCount++;
        if (repairToolCount == 1)
        {
            slotItems.Add(Item.REPAIR_TOOL);
        }
    }

    public void UseRepairTool()
    {
        repairToolCount--;
        if (repairToolCount == 0)
        {
            slotItems.Remove(Item.REPAIR_TOOL);
            if (equippedIndex >= slotItems.Count)
            {
                equippedIndex--;
            }
            equipQueue.Enqueue(slotItems[equippedIndex]);
            UpdateActiveItem();
        }
    }

    public void AddGrenade()
    {
        grenadeCount++;
        if (grenadeCount == 1)
        {
            slotItems.Add(Item.GRENADE);
        }
    }

    public void UseGrenade()
    {
        grenadeCount--;
        if (repairToolCount == 0)
        {
            slotItems.Remove(Item.GRENADE);
            if (equippedIndex >= slotItems.Count)
            {
                equippedIndex--;
            }
            equipQueue.Enqueue(slotItems[equippedIndex]);
            UpdateActiveItem();
        }
    }


}
