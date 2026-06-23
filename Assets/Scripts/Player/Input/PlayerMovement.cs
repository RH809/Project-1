/// <summary>
/// This script handles the player's movement inputs.
/// </summary>

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

    private PlayerStamina playerStamina;

    public bool IsJumping { get => jumping; }
    public bool IsSprinting { get => sprinting; }
    public bool IsGrounded { get => grounded; }
    public bool IsMoving { get => moving; }

    private Rigidbody rb;
    void Awake()
    {
        playerControls = new PlayerControls();
        playerStamina = GetComponent<PlayerStamina>();
        rb = GetComponent<Rigidbody>();

        playerAnimator.SetFloat("Speed", 1.0f);
    }

    void OnEnable() {
        Player.Instance.InputManager.Controls.Player.Sprint.performed += OnSprintPerformed;
        Player.Instance.InputManager.Controls.Player.Sprint.canceled += OnSprintCanceled;
        Player.Instance.InputManager.Controls.Player.Move.performed += OnMovePerformed;
        Player.Instance.InputManager.Controls.Player.Move.canceled += OnMoveCanceled;
        Player.Instance.InputManager.Controls.Player.Jump.performed += OnJumpPerformed;
        Player.Instance.InputManager.Controls.Player.Jump.canceled += OnJumpCanceled;
        /*
        Player.Instance.Controls.Player.Sprint.performed += OnSprintPerformed;
        Player.Instance.Controls.Player.Sprint.canceled += OnSprintCanceled;
        Player.Instance.Controls.Player.Move.performed += OnMovePerformed;
        Player.Instance.Controls.Player.Move.canceled += OnMoveCanceled;
        Player.Instance.Controls.Player.Jump.performed += OnJumpPerformed;
        Player.Instance.Controls.Player.Jump.canceled += OnJumpCanceled;
        */

        /*
        playerControls.Player.Sprint.performed += OnSprintPerformed;
        playerControls.Player.Sprint.canceled += OnSprintCanceled;
        playerControls.Player.Move.performed += OnMovePerformed;
        playerControls.Player.Move.canceled += OnMoveCanceled;
        playerControls.Player.Jump.performed += OnJumpPerformed;
        playerControls.Player.Jump.canceled += OnJumpCanceled;
        playerControls.Enable();
        */
        Health.OnDie += PlayerDeath;
    }

    void OnDisable() {
        Player.Instance.InputManager.Controls.Player.Sprint.performed -= OnSprintPerformed;
        Player.Instance.InputManager.Controls.Player.Sprint.canceled -= OnSprintCanceled;
        Player.Instance.InputManager.Controls.Player.Move.performed -= OnMovePerformed;
        Player.Instance.InputManager.Controls.Player.Move.canceled -= OnMoveCanceled;
        Player.Instance.InputManager.Controls.Player.Jump.performed -= OnJumpPerformed;
        Player.Instance.InputManager.Controls.Player.Jump.canceled -= OnJumpCanceled;
        /*
        Player.Instance.Controls.Player.Sprint.performed -= OnSprintPerformed;
        Player.Instance.Controls.Player.Sprint.canceled -= OnSprintCanceled;
        Player.Instance.Controls.Player.Move.performed -= OnMovePerformed;
        Player.Instance.Controls.Player.Move.canceled -= OnMoveCanceled;
        Player.Instance.Controls.Player.Jump.performed -= OnJumpPerformed;
        Player.Instance.Controls.Player.Jump.canceled -= OnJumpCanceled;
        */

        /*
        playerControls.Player.Sprint.performed -= OnSprintPerformed;
        playerControls.Player.Sprint.canceled -= OnSprintCanceled;
        playerControls.Player.Move.performed -= OnMovePerformed;
        playerControls.Player.Move.canceled -= OnMoveCanceled;
        playerControls.Player.Jump.performed -= OnJumpPerformed;
        playerControls.Player.Jump.canceled -= OnJumpCanceled;
        playerControls.Disable();
        */
        Health.OnDie -= PlayerDeath;
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
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx) {
        sprinting = false;
        playerAnimator.SetFloat("Speed", 1.0f);
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx) {
        jumpInput = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx) {
        jumpInput = false;
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
        rb.MovePosition(rb.position + movement * (sprinting && playerStamina.CanSprint ? sprintSpeed : walkSpeed) * Time.fixedDeltaTime);

    }

    void Update() {
        // Check for grounded
        grounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundMask
        ) && rb.linearVelocity.y <= 0.1f && Time.time - lastJumpTime >= 0.1f;
        playerAnimator.SetBool("Grounded", grounded);
        playerAnimator.SetBool("Moving", moving);
        if (jumpInput && !jumping && grounded)
        {
            // Jump
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumping = true;
            grounded = false;
            lastJumpTime = Time.time;
            playerAnimator.SetTrigger("Jump Start");
            playerAnimator.ResetTrigger("Jump End");
        }
        else if (jumping && grounded) {
            // Stop jumping
            playerAnimator.SetTrigger("Jump End");
            playerAnimator.ResetTrigger("Jump Start");
            jumping = false;
        }
    }

    void PlayerDeath(HealthContext healthContext)
    {
        if (healthContext.target.Equals(gameObject))
        {
            StopMovement();
        }
    }

    /// <summary>
    /// Cancels all movement inputs
    /// </summary>
    void StopMovement()
    {
        Debug.Log("Stopping player movement on death");
        jumpInput = false;
        sprinting = false;
        moving = false;
        moveInput = Vector2.zero;
    }


    /// <summary>
    /// Gizmos to show whether the player is grounded or not and the grounded check sphere
    /// </summary>
    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask)
            ? Color.green
            : Color.red;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
