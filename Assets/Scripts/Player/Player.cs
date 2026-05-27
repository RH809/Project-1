/// <summary>
/// This script creates a static instance reference to the player.
/// </summary>
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [HideInInspector] public Camera Camera;
    [HideInInspector] public Health Health;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Camera = GetComponentInChildren<Camera>();
        Health = GetComponent<Health>();
    }
}
