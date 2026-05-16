using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

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
}
