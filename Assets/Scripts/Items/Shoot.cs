/// <summary>
/// This script handles the bullet shooting.
/// </summary>

using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform bulletExit;

    [SerializeField] private GameObject bullet;

    [SerializeField] private GameObject[] disruptors;

    private Aim aim;
    private bool shooting = false;

    void Start()
    {
        aim = GetComponent<Aim>();
    }

    void Update()
    {
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
        /*
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
        */

    }
    
    /// <summary>
    /// Shoots a bullet in the direction of the target
    /// instantiates the bullet.
    /// </summary>
    private void ShootBullet() {

        Quaternion aimRotation = aim.GetTargetRotation(bulletExit);
        GameObject newBullet = Instantiate(bullet, bulletExit.position, aimRotation);
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
