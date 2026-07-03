using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{

    [SerializeField] private Transform interactSource;
    [SerializeField] private float interactRange;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float defenderInteractTime;
    [SerializeField] private float shopInteractTime;
    [SerializeField] private float powerUpInteractTime;

    [SerializeField] private Image interactProgress;
    [SerializeField] private GameObject interactTextObject;
    private TextMeshProUGUI interactText;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator repairToolAnimator;

    private bool defenderHover = false;
    private bool shopHover = false;
    private bool powerUpHover = false;
    private IInteractable interactable;
    private bool wasInteracting = false;
    private bool interacting = false;
    private float shopTimeout;
    private float interactTime;
    private float interactRequirement;

    private const string repairText = "Repair";
    private const string shopText = "Open Shop";
    private const string powerUpText = "Grab Power Up";
    Ray interactRay;

    void Start()
    {
        interactProgress.fillAmount = 0;
        interactTextObject.SetActive(false);
        interactText = interactTextObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        Player.Instance.InputManager.Controls.Player.Interact.performed += OnInteractPerformed;
        Player.Instance.InputManager.Controls.Player.Interact.canceled += OnInteractCanceled;
    }

    void OnDisable()
    {
        Player.Instance.InputManager.Controls.Player.Interact.performed -= OnInteractPerformed;
        Player.Instance.InputManager.Controls.Player.Interact.canceled -= OnInteractCanceled;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.State != UIManager.UIState.PLAY || GameManager.Instance.GameOver)
        {
            interacting = false;
            interactProgress.fillAmount = 0;
            if (UIManager.Instance.State == UIManager.UIState.SHOP)
            {
                shopTimeout = 0.05f;
            }
            playerAnimator.SetBool("Repair Tool Use", false);
            repairToolAnimator.SetBool("Repair Tool Use", false);
            wasInteracting = interacting;
            return;
        }
        shopTimeout = Mathf.Max(0, shopTimeout - Time.deltaTime);
        interactRay = new Ray(interactSource.position, interactSource.forward * interactRange);
        defenderHover = false;
        shopHover = false;
        powerUpHover = false;
        if (Physics.Raycast(interactRay, out RaycastHit hitInfo, interactRange, layerMask))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out interactable))
            {
                //Debug.Log($"{interactRange} raycast hit: {hitInfo.collider.gameObject}");
                switch (interactable.GetInteractType())
                {
                    case IInteractable.InteractType.DEFENDER:
                        if (Player.Instance.Inventory.EquippedItem == PlayerInventory.Item.REPAIR_TOOL &&
                            ((DefenderInteractable)interactable).Defender.Repairable)
                        {
                            defenderHover = true;
                            interactText.text = repairText;
                            interactText.fontSize = 36;
                            interactRequirement = defenderInteractTime;
                        }
                        break;
                    case IInteractable.InteractType.SHOP:
                        shopHover = true;
                        interactText.text = shopText;
                        interactText.fontSize = 36;
                        interactRequirement = shopInteractTime;
                        break;
                    case IInteractable.InteractType.POWER_UP:
                        powerUpHover = true;
                        interactText.text = powerUpText;
                        interactText.fontSize = 28;
                        interactRequirement = powerUpInteractTime;
                        break;
                }
            }
        }
        interactTextObject.SetActive(defenderHover || shopHover || powerUpHover);
        if ((!shopHover && !defenderHover && !powerUpHover) || shopTimeout > 0)
        {
            interacting = false;
            interactProgress.fillAmount = 0;
        }
        else if (wasInteracting && interacting)
        {
            interactTime += Time.deltaTime;
            interactProgress.fillAmount = Mathf.Clamp(interactTime / interactRequirement, 0, 360);

            if (interactTime >= interactRequirement)
            {
                interactable.Interact();
                interacting = false;
                interactProgress.fillAmount = 0;
            }
        }
        playerAnimator.SetBool("Repair Tool Use", defenderHover && interacting);
        repairToolAnimator.SetBool("Repair Tool Use", defenderHover && interacting);
        wasInteracting = interacting;
    }

    void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (UIManager.Instance.State != UIManager.UIState.PLAY || GameManager.Instance.GameOver) return;
        if (!shopHover && !defenderHover && !powerUpHover) return;
        if (!interacting)
        {
            interactTime = 0;
        }
        interacting = true;
    }

    void OnInteractCanceled(InputAction.CallbackContext ctx)
    {
        interacting = false;
        interactProgress.fillAmount = 0;
    }

    private void OnDrawGizmos()
    {
        if (defenderHover)
        {
            Gizmos.color = Color.lightBlue;
        }
        else if (shopHover)
        {
            Gizmos.color = Color.green;
        }
        else if (powerUpHover)
        {
            Gizmos.color = Color.orange;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawRay(interactRay);
    }
}

public interface IInteractable
{
    public enum InteractType
    {
        DEFENDER,
        SHOP,
        POWER_UP
    }
    public void Interact();
    public InteractType GetInteractType();
}
