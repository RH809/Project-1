/// <summary>
/// This script manages the UI for the map.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class MapUI : Singleton<MapUI>
{

    [SerializeField] private GameObject playerMapDot;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private RectTransform mapRect;

    private float mapWidth;
    private float mapHeight;
    protected override void Awake()
    {
        base.Awake();
        mapWidth = mapRect.rect.width;
        mapHeight = mapRect.rect.height;
    }

    void OnEnable()
    {
        UIManager.Instance.Input.UIControls.OpenMap.performed += OpenMap;
        UIManager.Instance.Input.UIControls.OpenMap.canceled += CloseMap;
    }

    void OnDisable()
    {
        UIManager.Instance.Input.UIControls.OpenMap.performed -= OpenMap;
        UIManager.Instance.Input.UIControls.OpenMap.canceled -= CloseMap;
    }

    void Update()
    {
        if (Player.Instance.Health.IsAlive)
        {
            playerMapDot.SetActive(true);
            // place the player map dot on the map in accordance to where the player is in the viewport
            Vector3 viewportPos = mapCamera.WorldToViewportPoint(Player.Instance.gameObject.transform.position);
            Vector2 uiPos = new Vector2(
                (viewportPos.x - 0.5f) * mapWidth,
                (viewportPos.y - 0.5f) * mapHeight
            );
            playerMapDot.transform.localPosition = uiPos;
        }
        else
        {
            playerMapDot.SetActive(false);
        }
    }

    public void OpenMap(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.GameOver) return;
        if (UIManager.Instance.State == UIManager.UIState.PLAY)
        {
            UIManager.Instance.SwitchState(UIManager.UIState.MAP);
        }
    }

    public void CloseMap(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.GameOver) return;
        if (UIManager.Instance.State == UIManager.UIState.MAP)
        {
            UIManager.Instance.SwitchState(UIManager.UIState.PLAY);
        }
    }
}
