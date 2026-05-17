using UnityEngine;

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

    private PlayerInput playerInput;
    private Vector2 lookInput;
    private float xRotation;
    void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.Player.Look.performed += ctx =>
        {
            lookInput = ctx.ReadValue<Vector2>();
        };

        playerInput.Player.Look.canceled += ctx =>
        {
            lookInput = Vector2.zero;
        };
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
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
