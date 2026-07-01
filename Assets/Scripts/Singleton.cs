/// <summary>
/// This script is the parent for all singletons.
/// </summary>
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
