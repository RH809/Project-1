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

    private PlayerControls playerControls;
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
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();

        playerAnimator.SetFloat("Speed", 1.0f);
    }

    void OnEnable() {
        playerControls.Player.Sprint.performed += OnSprintPerformed;
        playerControls.Player.Sprint.canceled += OnSprintCanceled;
        playerControls.Player.Move.performed += OnMovePerformed;
        playerControls.Player.Move.canceled += OnMoveCanceled;
        playerControls.Player.Jump.performed += OnJumpPerformed;
        playerControls.Player.Jump.canceled += OnJumpCanceled;
        playerControls.Enable();
    }

    void OnDisable() {
        playerControls.Player.Sprint.performed -= OnSprintPerformed;
        playerControls.Player.Sprint.canceled -= OnSprintCanceled;
        playerControls.Player.Move.performed -= OnMovePerformed;
        playerControls.Player.Move.canceled -= OnMoveCanceled;
        playerControls.Player.Jump.performed -= OnJumpPerformed;
        playerControls.Player.Jump.canceled -= OnJumpCanceled;
        playerControls.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) {
        moveInput = ctx.ReadValue<Vector2>();
        moving = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx) {
        moveInput = Vector2.zero;
        moving = false;
    }

    private void OnSprintPerformed(InputAction.CallbackContext ctx) {
        sprinting = true;
        playerAnimator.SetFloat("Speed", 1.33f);
        Debug.Log("Start sprinting");
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx) {
        sprinting = false;
        playerAnimator.SetFloat("Speed", 1.0f);
        Debug.Log("Stop sprinting");
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx) {
        jumpInput = true;
        Debug.Log("Started holding jump");
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx) {
        jumpInput = false;
        Debug.Log("Stopped holding jump");
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
