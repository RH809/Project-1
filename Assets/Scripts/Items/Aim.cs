/// <summary>
/// This script handles the aiming for shooting and throwing.
/// </summary>
using TMPro;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask targetMask;

    [SerializeField] private float defaultRange = 5.0f;
    [SerializeField] private GameObject[] disruptors;

    [SerializeField] private TextMeshProUGUI crosshair;

    public static float adjustedDisruptorHeight = 0.605f; // new height of disruptor hitbox when dead

    void Start()
    {
        crosshair.fontSize = SettingsManager.Instance.CrosshairSize;
    }

    void Update()
    {
        crosshair.fontSize = SettingsManager.Instance.CrosshairSize;
    }

    public Quaternion GetTargetRotation(Transform from)
    {
        return GetTargetRotation(playerCamera.ScreenPointToRay(Input.mousePosition), from);
    }

    /// <summary>
    /// Calculates the rotation to the target for aiming
    /// </summary>
    /// <param name="ray">Raycast ray for aiming</param>
    /// <param name="from">Position that the projectile is leaving from</param>
    /// <returns>Quaternion rotation to the target</returns>
    private Quaternion GetTargetRotation(Ray ray, Transform from)
    {
        RaycastHit hit;
        Quaternion aimRotation = Quaternion.identity;
        if (Physics.Raycast(ray, out hit, 50f, targetMask))
        {
            // Handle actual hitbox of walls
            GameObject hitObject = hit.collider.gameObject;
            // Handle lower hitbox of dead disruptors
            foreach (GameObject obj in disruptors)
            {
                if (obj.Equals(hitObject) && !obj.GetComponent<Disruptor>().IsAlive && hit.point.y > adjustedDisruptorHeight + GameManager.GroundY)
                {
                    Vector3 rayDir = ray.direction;
                    ray = new Ray(hit.point + 0.1f * rayDir, rayDir);
                    return GetTargetRotation(ray, from); // recursive call with new ray
                }
            }
            // Calculate angle to raycast hit
            Vector3 path = hit.point - from.transform.position;
            if (path.magnitude < 0.5f)
            { // do default path if too close
                Vector3 end = playerCamera.transform.position + ray.direction * defaultRange;
                aimRotation = Quaternion.LookRotation(end - from.transform.position, Vector3.up);
            }
            else
            {
                aimRotation = Quaternion.LookRotation(path, Vector3.up);
                Debug.DrawRay(from.transform.position, path, Color.blue);
            }
        }
        else
        {
            // Calculate angle based on default range
            Vector3 end = playerCamera.transform.position + ray.direction * defaultRange;
            aimRotation = Quaternion.LookRotation(end - from.transform.position, Vector3.up);
        }
        return aimRotation;
    }
}
