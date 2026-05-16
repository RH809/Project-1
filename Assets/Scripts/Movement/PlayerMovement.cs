using UnityEditor;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float backwardMultiplier = 0.6f;
    [SerializeField] private float sidewaysMultiplier = 0.75f;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private PlayerInput playerInput;
    private Vector2 moveInput;
    private bool jumpInput = false;
    private float lastJumpTime = 0.0f;
    private bool moving = false;
    private bool sprinting = false;
    private bool jumping = false;
    private bool grounded = false;

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody>();

        playerInput.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
            moving = true;
        };
        playerInput.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
            moving = false;
        };

        playerInput.Player.Sprint.performed += ctx =>
        {
            sprinting = true;
            playerAnimator.SetFloat("Speed", 1.33f);
            Debug.Log("Start sprinting");
        };
        playerInput.Player.Sprint.canceled += ctx =>
        {
            sprinting = false;
            playerAnimator.SetFloat("Speed", 1.0f);
            Debug.Log("Stop sprinting");
        };

        playerInput.Player.Jump.performed += ctx =>
        {
            jumpInput = true;
            Debug.Log("Started holding jump");
        };
        playerInput.Player.Jump.canceled += ctx =>
        {
            jumpInput = false;
            Debug.Log("Stopped holding jump");
        };

        playerAnimator.SetFloat("Speed", 1.0f);
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
        if (movement.Equals(Vector3.zero))
        {
            moving = false;
        }

        movement = transform.TransformDirection(movement);
        rb.MovePosition(rb.position + movement * (sprinting ? sprintSpeed : walkSpeed) * Time.fixedDeltaTime);

    }

    void Update() {
        grounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundMask
        ) && rb.linearVelocity.y <= 0.1f && Time.time - lastJumpTime >= 0.1f;
        playerAnimator.SetBool("Grounded", grounded);
        playerAnimator.SetBool("Moving", moving);
        if (jumpInput && !jumping && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumping = true;
            grounded = false;
            lastJumpTime = Time.time;
            Debug.Log("Jumping");
            playerAnimator.SetTrigger("Jump Start");
            playerAnimator.ResetTrigger("Jump End");
        }
        else if (jumping && grounded) {
            // stop jumping
            Debug.Log("Ending jump");
            playerAnimator.SetTrigger("Jump End");
            playerAnimator.ResetTrigger("Jump Start");
            jumping = false;
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask)
            ? Color.green
            : Color.red;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
