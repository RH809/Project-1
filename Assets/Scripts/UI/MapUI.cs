using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public static MapUI Instance;

    [SerializeField] private GameObject playerMapDot;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private RectTransform mapRect;

    private float mapWidth;
    private float mapHeight;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        mapWidth = mapRect.rect.width;
        mapHeight = mapRect.rect.height;
    }

    void OnEnable()
    {
        Player.Instance.InputManager.Controls.Player.OpenMap.performed += OpenMap;
        Player.Instance.InputManager.Controls.Player.OpenMap.canceled += CloseMap;
    }

    void OnDisable()
    {
        Player.Instance.InputManager.Controls.Player.OpenMap.performed -= OpenMap;
        Player.Instance.InputManager.Controls.Player.OpenMap.canceled -= CloseMap;
    }

    void Update()
    {
        if (Player.Instance.Health.IsAlive)
        {
            playerMapDot.SetActive(true);
            Vector3 viewportPos = mapCamera.WorldToViewportPoint(Player.Instance.gameObject.transform.position);
            Vector2 uiPos = new Vector2(
                (viewportPos.x - 0.5f) * mapWidth,
                (viewportPos.y - 0.5f) * mapHeight
            );
            playerMapDot.transform.position = uiPos;
        }
        else
        {
            playerMapDot.SetActive(false);
        }
    }

    public void OpenMap(InputAction.CallbackContext ctx)
    {
        if (UIManager.Instance.State == UIManager.UIState.PLAY)
        {
            UIManager.Instance.SwitchState(UIManager.UIState.MAP);
        }
    }

    public void CloseMap(InputAction.CallbackContext ctx)
    {
        if (UIManager.Instance.State == UIManager.UIState.MAP)
        {
            UIManager.Instance.SwitchState(UIManager.UIState.PLAY);
        }
    }
}
