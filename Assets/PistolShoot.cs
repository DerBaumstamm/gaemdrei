using UnityEngine;

public class PistolShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float shootForce = 20f;
    public float gravity = 9.8f; // Adjust gravity for the realistic ballistic trajectory
    public float fireRate = 0.5f;

    private float nextFireTime = 0f;

    void Update()
    {
        // Check if the Fire1 button is pressed (you can customize this input)
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        // Create a new bullet at the bulletSpawnPoint position and rotation
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Get the rigidbody of the bullet and apply force forward with gravity
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.AddForce(bulletSpawnPoint.forward * shootForce, ForceMode.Impulse);
            bulletRb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }

        // Destroy the bullet after a certain time (adjust as needed)
        Destroy(bullet, 3f);
    }
}
