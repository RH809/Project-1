/// <summary>
/// This script handles the player respawn logic.
/// </summary>
using System.Collections;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public static Respawn Instance;
    [HideInInspector] public Camera Camera;
    [SerializeField] private Transform respawnTransform;
    [SerializeField] private float baseRespawnTime;

    private float respawnTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Camera = GetComponentInChildren<Camera>(true);
    }

    void Start()
    {
        Camera.gameObject.SetActive(false);
        respawnTime = baseRespawnTime;
    }

    void OnEnable()
    {
        Health.OnDie += PlayerDie;
    }

    void OnDisable()
    {
        Health.OnDie -= PlayerDie;
    }

    void PlayerDie(HealthContext healthCtx)
    {
        if (healthCtx.target.Equals(Player.Instance.gameObject))
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    /// <summary>
    /// Handles the respawn routine
    /// </summary>
    IEnumerator RespawnRoutine()
    {
        Debug.Log("Player died. Waiting 5 seconds for respawn...");
        // Move player to respawn location
        Player.Instance.gameObject.transform.position = respawnTransform.position;
        Player.Instance.gameObject.transform.rotation = respawnTransform.rotation;
        // Disable player and switch cameras
        Player.Instance.gameObject.SetActive(false);
        Camera.gameObject.SetActive(true);
        

        yield return new WaitForSeconds(respawnTime);

        Debug.Log("Player respawned!");
        // Enable player and switch cameras
        Camera.gameObject.SetActive(false);
        Player.Instance.gameObject.SetActive(true);
        Player.Instance.Health.Respawn();
    }
}
