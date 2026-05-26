/// <summary>
/// This script creates a static instance reference to the player.
/// </summary>
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [HideInInspector] public Camera MainCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        MainCamera = GetComponentInChildren<Camera>();
    }
}
