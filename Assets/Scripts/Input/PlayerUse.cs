using System.Collections.Generic;
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
    private Queue<item> equipQueue;

    [SerializeField] private Canvas aimHUD;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject gun;
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
        if ((value == 0 && equippedItem == item.SWORD) ||
            (value == 1 && equippedItem == item.GUN) ||
            (value == 2 && equippedItem == item.TRAP) ||
            (value == 3 && equippedItem == item.TURRET)) {
            return;
        }
        switch (value) {
            case 0:
                playerAnimator.SetTrigger("Equip Sword");
                equipQueue.Enqueue(item.SWORD);
                //equippedItem = item.SWORD;
                break;
            case 1:
                playerAnimator.SetTrigger("Equip Gun");
                equipQueue.Enqueue(item.GUN);
                //equippedItem = item.GUN;
                break;
            case 2:

            case 3:

            default:
                break;
        }
        Debug.Log(value + " " + (int)equippedItem);
    }

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
        if (!(equippedItem == item.SWORD && isInSwingAnimation()) &&
            !(equippedItem == item.GUN && isInShootAnimation())) {
            while (equipQueue.Count > 0) {
                equippedItem = equipQueue.Dequeue();
            }
        }
        sword.SetActive(equippedItem == item.SWORD);
        gun.SetActive(equippedItem == item.GUN);
        aimHUD.enabled = (equippedItem == item.SWORD || equippedItem == item.GUN);
    }

    private bool isInSwingAnimation() {
        return playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Sword Swing 1") || playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Sword Swing 2") ||
            playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Sword Swing 3") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Sword Swing 1") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Sword Swing 2") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Idle -> Sword Swing 3") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Swing 1 -> Sword Idle") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Swing 2 -> Sword Idle") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Sword Swing 3 -> Sword Idle");
    }

    private bool isInShootAnimation() {
        return playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Gun Shoot") || playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Idle -> Gun Shoot") ||
            playerAnimator.GetAnimatorTransitionInfo(1).IsName("Gun Shoot -> Gun Idle");
    }

    void Use()
    {
        if (cooldownTime <= 0.0f)
        {
            switch (equippedItem)
            {
                case item.SWORD:
                    if (swordHitbox.isSwinging() || isInSwingAnimation()) {
                        break;
                    }
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
                case item.GUN:
                    if (shoot.isShooting() || isInShootAnimation()) {
                        break;
                    }
                    cooldownTime = gunCooldown;
                    playerAnimator.SetTrigger("Shoot");
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
