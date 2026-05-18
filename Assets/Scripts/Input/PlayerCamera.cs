using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform leftShoulder;
    [SerializeField] private Transform rightShoulder;
    [SerializeField] private float aimAmount = 0.6f;

    [SerializeField] private float sensitivity = 60f;
    [SerializeField] private float maxYRotation = 60f;
    [SerializeField] private float minYRotation = -80f;

    private PlayerControls playerControls;
    private Vector2 lookInput;
    private float xRotation;
    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Player.Look.performed += OnLookPerformed;
        playerControls.Player.Look.canceled += OnLookCanceled;
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Player.Look.performed -= OnLookPerformed;
        playerControls.Player.Look.canceled -= OnLookCanceled;
        playerControls.Disable();
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx) {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx) {
        lookInput = Vector2.zero;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        // Rotate player left/right
        Debug.Log(mouseX);
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minYRotation, maxYRotation);

        cameraTransform.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);
    }

    void LateUpdate()
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


}
