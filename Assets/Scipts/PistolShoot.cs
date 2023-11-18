using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PistolShoot : MonoBehaviour
{
    public InputAction shootAction;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float shootForce = 20f;
    public float recoilAngle = 5f;
    public float recoverySpeed = 5f;
    public float gravity = 9.8f;
    public float fireRate = 0.5f;

    private float nextFireTime = 0f;
    private Quaternion initialRotation; // Store the initial rotation

    private void OnEnable()
    {
        shootAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    void Start()
    {
        // Store the initial rotation when the script starts
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (shootAction.triggered)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // Rotate the pistol back down gradually
        transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, recoverySpeed * Time.deltaTime);
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

        // Apply recoil by rotating the pistol upward
        transform.localRotation = Quaternion.Euler(-recoilAngle, initialRotation.y, initialRotation.z);

        // Destroy the bullet after a certain time (adjust as needed)
        Destroy(bullet, 3f);
    }
}
