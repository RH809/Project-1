/// <summary>
/// This script handles the player's camera control.
/// </summary>

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform leftShoulder;
    [SerializeField] private Transform rightShoulder;
    [SerializeField] private float aimAmount = 0.6f;
    [SerializeField] private bool enableCameraLock;

    [SerializeField] private float sensitivity = 60f;
    [SerializeField] private float maxYRotation = 60f;
    [SerializeField] private float minYRotation = -80f;

    private Vector2 lookInput;
    private float xRotation;

    private bool lockedCamera = false;

    void OnEnable()
    {
        Player.Instance.InputManager.Controls.Player.Look.performed += OnLookPerformed;
        Player.Instance.InputManager.Controls.Player.Look.canceled += OnLookCanceled;
    }

    void OnDisable()
    {
        Player.Instance.InputManager.Controls.Player.Look.performed -= OnLookPerformed;
        Player.Instance.InputManager.Controls.Player.Look.canceled -= OnLookCanceled;
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx) {
        if (GameManager.Instance.GameOver) return;
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx) {
        if (GameManager.Instance.GameOver) return;
        lookInput = Vector2.zero;
    }

    void Update()
    {
        if ((UIManager.Instance.State != UIManager.UIState.PLAY && UIManager.Instance.State != UIManager.UIState.MAP) ||
            (lockedCamera && enableCameraLock) || GameManager.Instance.GameOver) return;
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        // Rotate player left/right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minYRotation, maxYRotation);

        cameraTransform.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void UpdateArmRotation()
    {
        Vector3 worldAxis = playerTransform.right;

        // convert into shoulder parent space
        Vector3 localAxis =
            leftShoulder.parent.InverseTransformDirection(worldAxis);

        Quaternion aimOffset =
            Quaternion.AngleAxis(xRotation * aimAmount, localAxis);

        Quaternion baseLeft = leftShoulder.localRotation;
        Quaternion baseRight = rightShoulder.localRotation;

        leftShoulder.localRotation = aimOffset * baseLeft;
        rightShoulder.localRotation = aimOffset * baseRight;
    }

    public void LockCamera() {
        lockedCamera = true;
    }

    public void UnlockCamera() {
        lockedCamera = false;
    }
}
