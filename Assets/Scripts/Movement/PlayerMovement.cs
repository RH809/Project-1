using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float backwardMultiplier = 0.6f;
    [SerializeField] private float sidewaysMultiplier = 0.75f;

    private PlayerInput playerInput;
    private Vector2 moveInput;
    private bool sprinting = false;

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody>();

        playerInput.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
            if (sprinting)
            {
                playerAnimator.SetFloat("Speed", 2.0f);
            }
            else
            {
                playerAnimator.SetFloat("Speed", 1.0f);
            }
        };
        playerInput.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
            playerAnimator.SetFloat("Speed", 0.0f);
        };

        playerAnimator.SetFloat("Speed", 0.0f);

        playerInput.Player.Sprint.performed += ctx =>
        {
            sprinting = true;
            Debug.Log("Start sprinting");
        };
        playerInput.Player.Sprint.canceled += ctx =>
        {
            sprinting = false;
            Debug.Log("Stop sprinting");
        };
        
    }

    void OnEnable() {
        playerInput.Enable();
    }

    void OnDisable() {
        playerInput.Disable();
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3(
            moveInput.x * sidewaysMultiplier,
            0,
            (moveInput.y >= 0 ? moveInput.y : moveInput.y * backwardMultiplier)
        );

        movement = transform.TransformDirection(movement);
        rb.MovePosition(rb.position + movement * (sprinting ? sprintSpeed : walkSpeed) * Time.fixedDeltaTime);

    }
}
