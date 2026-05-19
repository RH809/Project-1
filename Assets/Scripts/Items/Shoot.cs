using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform bulletExit;

    [SerializeField] private GameObject bullet;
    private void ShootBullet() {
        GameObject newBullet = Instantiate(bullet, bulletExit);
        Debug.Log("Shot " + newBullet);
    }

    public void ShootStart() {
        ShootBullet();
    }
}
