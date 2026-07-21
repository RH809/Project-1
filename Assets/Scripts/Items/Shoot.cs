/// <summary>
/// This script handles the bullet shooting.
/// </summary>

using UnityEngine;
using UnityEngine.Pool;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform bulletExit;

    [SerializeField] private Bullet bullet;

    [SerializeField] private GameObject[] disruptors;
    private ObjectPool<Bullet> bulletPool;

    private Aim aim;
    private bool shooting = false;

    void Start()
    {
        aim = GetComponent<Aim>();
        bulletPool = new ObjectPool<Bullet>(
            CreateBullet,
            OnTakeBullet,
            OnReturnBullet,
            OnDestroyBullet,
            true,
            5,
            20
        );
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
        //GameObject newBullet = Instantiate(bullet.gameObject, bulletExit.position, aimRotation);
        //newBullet.GetComponent<Bullet>().SetDisruptors(disruptors);
        //Debug.Log("Created new bullet");
        Bullet bullet = bulletPool.Get();
        bullet.transform.SetPositionAndRotation(bulletExit.position, aimRotation);
        bullet.Spawn(bulletExit.position, aimRotation);
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

    private Bullet CreateBullet()
    {
        Bullet newBullet = Instantiate(bullet);
        newBullet.SetDisruptors(disruptors);
        newBullet.SetPool(bulletPool);
        return newBullet;
    }

    private void OnTakeBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        //Debug.Log("Returning bullet");
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
