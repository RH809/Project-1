/// <summary>
/// This script acts as the singleton reference for the world space canvas.
/// </summary>
using UnityEngine;

public class WorldSpaceCanvas : MonoBehaviour
{
    public static WorldSpaceCanvas Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
