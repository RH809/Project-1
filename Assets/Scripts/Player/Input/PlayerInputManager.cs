/// <summary>
/// This script manages the player controls.
/// </summary>
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    
    public PlayerControls Controls { get => playerControls; }

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }
}
