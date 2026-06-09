/// <summary>
/// This script handles the bullet shooting.
/// </summary>

using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform bulletExit;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private GameObject bullet;
    [SerializeField] private LayerMask targetMask;

    [SerializeField] private float defaultRange = 5.0f;

    [SerializeField] private GameObject[] disruptors;

    public static float adjustedDisruptorHeight = 0.605f; // new height of disruptor hitbox when dead
    public static float wallHeight = 1.042f; // height of walls

    private bool shooting = false;

    
    void Update() {
        /*
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(playerCamera.transform.position, ray.direction, Color.red);
        Vector3 test = playerCamera.transform.position + ray.direction * 5f;
        Debug.DrawRay(playerCamera.transform.position, test - playerCamera.transform.position, Color.green);
        Debug.DrawRay(bulletExit.transform.position, test - bulletExit.transform.position, Color.orange);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Transform objectHit = hit.transform;
            Vector3 path = hit.point - bulletExit.transform.position;
            Quaternion aimRotation = Quaternion.LookRotation(path, Vector3.up);
            Debug.DrawRay(bulletExit.transform.position, path, Color.blue);
            
        }
        */
        // Debug raycasts
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50f, targetMask))
        {
            // Calculate angle to raycast hit
            Vector3 path = hit.point - bulletExit.transform.position;
            Debug.DrawRay(bulletExit.transform.position, path, Color.blue);
        }
        else
        {
            // Calculate angle based on default range
            Vector3 end = playerCamera.transform.position + ray.direction * defaultRange;
            Debug.DrawRay(bulletExit.transform.position, end - bulletExit.transform.position, Color.blue);
        }

    }

    /// <summary>
    /// Calculates the rotation to the target for shooting
    /// </summary>
    /// <param name="ray">Raycast ray for shooting</param>
    /// <returns>Quaternion rotation to the target</returns>
    private Quaternion GetTargetRotation(Ray ray)
    {
        RaycastHit hit;
        Quaternion aimRotation = Quaternion.identity;
        if (Physics.Raycast(ray, out hit, 50f, targetMask))
        {
            // Handle actual hitbox of walls
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.layer == LayerMask.NameToLayer("Walls") && hit.point.y > wallHeight + GameManager.GroundY)
            {
                Vector3 rayDir = ray.direction;
                ray = new Ray(hit.point, rayDir);
                return GetTargetRotation(ray); // recursive call with new ray
            }
            // Handle lower hitbox of dead disruptors
            foreach (GameObject obj in disruptors)
            {
                if (obj.Equals(hitObject) && !obj.GetComponent<Disruptor>().IsAlive && hit.point.y > adjustedDisruptorHeight + GameManager.GroundY)
                {
                    Vector3 rayDir = ray.direction;
                    ray = new Ray(hit.point, rayDir);
                    return GetTargetRotation(ray); // recursive call with new ray
                }
            }
            // Calculate angle to raycast hit
            Vector3 path = hit.point - bulletExit.transform.position;
            aimRotation = Quaternion.LookRotation(path, Vector3.up);
            Debug.DrawRay(bulletExit.transform.position, path, Color.blue);
        }
        else
        {
            // Calculate angle based on default range
            Vector3 end = playerCamera.transform.position + ray.direction * defaultRange;
            aimRotation = Quaternion.LookRotation(end - bulletExit.transform.position, Vector3.up);
        }
        return aimRotation;
    }
    
    /// <summary>
    /// Shoots a bullet in the direction of the target
    /// instantiates the bullet.
    /// </summary>
    private void ShootBullet() {
        /*
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        
        Quaternion aimRotation = Quaternion.identity;
        if (Physics.Raycast(ray, out hit, 50f, targetMask)) // Raycast to see if there is any target being aimed at
        {
            GameObject hitObject = hit.collider.gameObject;
            foreach (GameObject obj in disruptors)
            {
                // Handle lower hitbox of dead disruptors
                if (obj.Equals(hitObject) && !obj.GetComponent<Disruptor>().IsAlive && hit.point.y > adjustedDisruptorHeight)
                {
                    bool hitSomething = true;
                    do {
                        Vector3 rayDir = ray.direction;
                        ray = new Ray(hit.point, rayDir);
                        hitSomething = Physics.Raycast(ray, out hit, 50f, targetMask);
                        hitObject = hit.collider.gameObject;
                    } while (hitSomething && obj.Equals(hitObject) && hit.point.y > adjustedDisruptorHeight);

                    if (!hitSomething)
                    {
                        Vector3 end = playerCamera.transform.position + ray.direction * defaultRange;
                        aimRotation = Quaternion.LookRotation(end - bulletExit.transform.position, Vector3.up);
                    }
                }
            }
            // Calculate angle to raycast hit
            Vector3 path = hit.point - bulletExit.transform.position;
            aimRotation = Quaternion.LookRotation(path, Vector3.up);
            Debug.DrawRay(bulletExit.transform.position, path, Color.blue);
        }
        else
        {
            // Calculate angle based on default range
            Vector3 end = playerCamera.transform.position + ray.direction * defaultRange;
            aimRotation = Quaternion.LookRotation(end - bulletExit.transform.position, Vector3.up);
        }
        */
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Quaternion aimRotation = GetTargetRotation(ray);

        GameObject newBullet = Instantiate(bullet, bulletExit.transform.position, aimRotation);
        newBullet.GetComponent<Bullet>().SetDisruptors(disruptors);
    }

    public void ShootStart() {
        //Debug.Log("Shoot start");
        shooting = true;
        ShootBullet();
    }

    public void ShootEnd() {
        //Debug.Log("Shoot end");
        shooting = false;
    }

    public bool isShooting() {
        return shooting;
    }
}
