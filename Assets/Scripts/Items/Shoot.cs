using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform bulletExit;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private GameObject bullet;

    private bool shooting = false;
    private void ShootBullet() {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(playerCamera.transform.position, ray.direction, Color.red);
        
        Quaternion aimRotation = Quaternion.identity;
        Debug.Log(ray);
        if (Physics.Raycast(ray, out hit))
        {
            //Transform objectHit = hit.transform;
            Vector3 path = hit.point - bulletExit.transform.position;
            aimRotation = Quaternion.LookRotation(path, Vector3.up);
            Debug.DrawRay(bulletExit.transform.position, path, Color.blue);
        }
        
        GameObject newBullet = Instantiate(bullet, bulletExit.transform.position, aimRotation);
        Debug.Log("Shot " + newBullet);
    }

    public void ShootStart() {
        Debug.Log("Shoot start");
        shooting = true;
        ShootBullet();
    }

    public void ShootEnd() {
        Debug.Log("Shoot end");
        shooting = false;
    }

    public bool isShooting() {
        return shooting;
    }
}
