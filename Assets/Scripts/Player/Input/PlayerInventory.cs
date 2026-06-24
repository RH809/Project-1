/// <summary>
/// This script manages the player's held item and handles using the items.
/// </summary>

using System.Collections.Generic;
using UnityEditor;
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
    private int equippedIndex;
    public int EquippedIndex { get => equippedIndex; }
    private Queue<Item> equipQueue;
    private List<Item> slotItems;
    private int repairToolCount = 0;
    private int grenadeCount = 0;
    private int potionCount = 0;
    

    [SerializeField] private Canvas aimHUD;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject repairTool;
    [SerializeField] private GameObject grenade;
    [SerializeField] private GameObject potion;

    private PlayerCamera playerCamera;
    private SwordHitbox swordHitbox;
    private Shoot shoot;
    private GrenadeThrow grenadeThrow;
    private PotionDrink potionDrink;

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
        potionDrink = GetComponent<PotionDrink>();
    }
    void Start()
    {
        slotItems = new List<Item>();
        slotItems.Add(Item.SWORD);
        slotItems.Add(Item.GUN);
        // ==== TESTING ======
        AddRepairTool();
        for (int i = 0; i < 5; i++) AddGrenade();
        AddPotion();
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

        Health.OnDie += OnDie;
        Health.OnRespawn += OnRespawn;
        
    }

    void OnDisable()
    {
        Player.Instance.InputManager.Controls.Player.Use.performed -= OnUsePerformed;
        Player.Instance.InputManager.Controls.Player.SelectItem.performed -= OnSelectItemPerformed;
        Player.Instance.InputManager.Controls.Player.Scroll.performed -= OnScrollPerformed;

        Health.OnDie -= OnDie;
        Health.OnRespawn -= OnRespawn;

    }

    private void OnUsePerformed(InputAction.CallbackContext ctx) {
        if (UIManager.Instance.State == UIManager.UIState.PLAY)
        {
            Use();
        }
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
            case 4:
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
            !(equippedItem == Item.GRENADE && IsInThrowAnimation()) &&
            !(equippedItem == Item.HEALTH_POTION && IsInDrinkAnimation()))
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
                        playerAnimator.ResetTrigger("Equip Potion");
                        break;
                    case Item.GUN:
                        //Debug.Log("Equip gun animation");
                        playerAnimator.SetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Grenade");
                        playerAnimator.ResetTrigger("Equip Repair Tool");
                        playerAnimator.ResetTrigger("Equip Potion");
                        break;
                    case Item.GRENADE:
                        //Debug.Log("Equip grenade animation");
                        playerAnimator.SetTrigger("Equip Grenade");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Repair Tool");
                        playerAnimator.ResetTrigger("Equip Potion");
                        break;
                    case Item.REPAIR_TOOL:
                        //Debug.Log("Equip repair tool animation");
                        playerAnimator.SetTrigger("Equip Repair Tool");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Grenade");
                        playerAnimator.ResetTrigger("Equip Potion");
                        break;
                    case Item.HEALTH_POTION:
                        //Debug.Log("Equip potion animation");
                        playerAnimator.SetTrigger("Equip Potion");
                        playerAnimator.ResetTrigger("Equip Sword");
                        playerAnimator.ResetTrigger("Equip Gun");
                        playerAnimator.ResetTrigger("Equip Grenade");
                        playerAnimator.ResetTrigger("Equip Repair Tool");
                        break;
                }
            }
            
        }
        sword.SetActive(equippedItem == Item.SWORD);
        gun.SetActive(equippedItem == Item.GUN);
        repairTool.SetActive(equippedItem == Item.REPAIR_TOOL);
        grenade.SetActive(equippedItem == Item.GRENADE && !grenadeThrow.ThrowingGrenade);
        potion.SetActive(equippedItem == Item.HEALTH_POTION && !potionDrink.FinishedDrinking);
        //aimHUD.enabled = (equippedItem != Item.HEALTH_POTION);
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
    /// Checks if the player is currently in the potion drink animation
    /// </summary>
    /// <returns>Whether the player is in the potion drink animation</returns>
    private bool IsInDrinkAnimation()
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Potion Drink") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Potion Idle -> Potion Drink") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Potion Drink -> Potion Idle") || playerAnimator.GetBool("Potion Drink");
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
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Potion Idle") ||

            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Grenade Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Repair Tool Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Potion Idle") ||

            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Idle -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Idle -> Gun Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Idle -> Repair Tool Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Grenade Idle -> Potion Idle") ||

            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Repair Tool Idle -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Repair Tool Idle -> Gun Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Repair Tool Idle -> Grenade Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Repair Tool Idle -> Potion Idle") ||

            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Potion Idle -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Potion Idle -> Gun Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Potion Idle -> Grenade Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Potion Idle -> Repair Tool Idle");
            
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
                case Item.HEALTH_POTION:
                    if (IsInDrinkAnimation() || Player.Instance.Health.CurrentHealth == Player.Instance.Health.MaxHealth)
                    { // don't use if already in drinking animation or at full health
                        break;
                    }
                    playerAnimator.SetTrigger("Potion Drink");
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
            PlayerHUD.Instance.AddItem(Item.REPAIR_TOOL);
        }
        else
        {
            PlayerHUD.Instance.IncrementItem(slotItems.IndexOf(Item.REPAIR_TOOL));
        }
    }

    public void UseRepairTool()
    {
        repairToolCount--;
        if (repairToolCount == 0)
        {
            slotItems.Remove(Item.REPAIR_TOOL);
            PlayerHUD.Instance.RemoveItem(equippedIndex);
            if (equippedIndex >= slotItems.Count)
            {
                equippedIndex--;
            }
            equipQueue.Enqueue(slotItems[equippedIndex]);
            UpdateActiveItem();
        }
        else
        {
            PlayerHUD.Instance.DecrementItem(equippedIndex);
        }
    }

    public void AddGrenade()
    {
        grenadeCount++;
        if (grenadeCount == 1)
        {
            slotItems.Add(Item.GRENADE);
            PlayerHUD.Instance.AddItem(Item.GRENADE);
        }
        else
        {
            PlayerHUD.Instance.IncrementItem(slotItems.IndexOf(Item.GRENADE));
        }
    }

    public void UseGrenade()
    {
        grenadeCount--;
        if (grenadeCount == 0)
        {
            slotItems.Remove(Item.GRENADE);
            PlayerHUD.Instance.RemoveItem(equippedIndex);
            if (equippedIndex >= slotItems.Count)
            {
                equippedIndex--;
            }
            equipQueue.Enqueue(slotItems[equippedIndex]);
            UpdateActiveItem();
        }
        else
        {
            PlayerHUD.Instance.DecrementItem(equippedIndex);
        }
    }

    public void AddPotion()
    {
        potionCount++;
        if (potionCount == 1)
        {
            slotItems.Add(Item.HEALTH_POTION);
            PlayerHUD.Instance.AddItem(Item.HEALTH_POTION);
        }
        else
        {
            PlayerHUD.Instance.IncrementItem(slotItems.IndexOf(Item.HEALTH_POTION));
        }
    }

    public void UsePotion()
    {
        potionCount--;
        if (potionCount == 0)
        {
            slotItems.Remove(Item.HEALTH_POTION);
            PlayerHUD.Instance.RemoveItem(equippedIndex);
            if (equippedIndex >= slotItems.Count)
            {
                equippedIndex--;
            }
            equipQueue.Enqueue(slotItems[equippedIndex]);
            UpdateActiveItem();
        }
        else
        {
            PlayerHUD.Instance.DecrementItem(equippedIndex);
        }
    }

    public void OnDie(HealthContext healthContext)
    {
        if (healthContext.target.Equals(Player.Instance.gameObject))
        {
            equipQueue.Clear();
        }
    }

    public void OnRespawn(HealthContext healthContext)
    {
        if (healthContext.target.Equals(Player.Instance.gameObject))
        {
            Debug.Log("Player respawn inventory...");
            equipQueue.Enqueue(slotItems[equippedIndex]);
            equippedIndex = 0;
            equippedItem = Item.SWORD; // set to default so that animation transition will play on respawn
        }
    }

}
