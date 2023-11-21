using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PistolShoot : NetworkBehaviour
{
    [SerializeField] private InputAction shootAction;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float recoverySpeed = 5f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float recoilAngle = 5f;

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

    public override void OnNetworkSpawn()
    {
       initialRotation = transform.rotation;
    }

    void Update()
    {
        if(!IsOwner) return;

        if (shootAction.triggered)
        {
            ShootServerRpc();
            transform.localRotation = Quaternion.Euler(-recoilAngle, initialRotation.y, initialRotation.z);
            nextFireTime = Time.time + 1f / fireRate;
        }

        // Rotate the pistol back down gradually
        transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, recoverySpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        //Transform bulletPrefabTransform = Instantiate(bulletPrefab);
        //bulletPrefabTransform.GetComponent<NetworkObject>().Spawn(true);
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject bulletPrefabTransform = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bulletPrefabTransform.GetComponent<NetworkObject>().Spawn(true);
    }

    /*
    void Shoot()
    {
        // Create a new bullet at the bulletSpawnPoint position and rotation
        Transform bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<NetworkObject>().Spawn(true);
        
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
    */
}
