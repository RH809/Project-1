using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static float GroundY = -0.5f;


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
