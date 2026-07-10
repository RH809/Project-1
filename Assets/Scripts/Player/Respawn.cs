/// <summary>
/// This script handles the player respawn logic.
/// </summary>
using System.Collections;
using TMPro;
using UnityEngine;

public class Respawn : Singleton<Respawn>
{
    [HideInInspector] public Camera Camera;
    [SerializeField] private Transform respawnTransform;
    [SerializeField] private int baseRespawnTime;
    [SerializeField] private Canvas respawnCanvas;
    [SerializeField] private TextMeshProUGUI respawnText;

    private int respawnTime;

    protected override void Awake()
    {
        base.Awake();
        Camera = GetComponentInChildren<Camera>(true);
    }

    void Start()
    {
        Camera.gameObject.SetActive(false);
        respawnTime = baseRespawnTime;
        respawnCanvas.enabled = false;
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
        //Debug.Log("Player died. Waiting 5 seconds for respawn...");
        // Move player to respawn location
        Player.Instance.gameObject.transform.position = respawnTransform.position;
        Player.Instance.gameObject.transform.rotation = respawnTransform.rotation;
        // Disable player and switch cameras
        Player.Instance.gameObject.SetActive(false);
        Camera.gameObject.SetActive(true);
        respawnCanvas.enabled = true;
        for (int i = respawnTime; i > 0; i--)
        {
            respawnText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        

        //yield return new WaitForSeconds(respawnTime);

        //Debug.Log("Player respawned!");
        // Enable player and switch cameras
        Camera.gameObject.SetActive(false);
        Player.Instance.gameObject.SetActive(true);
        respawnCanvas.enabled = false;
        Player.Instance.Health.Respawn(1, false);
    }
}
