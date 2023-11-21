using Unity.Netcode;
using UnityEngine;

public class BulletBehavior : NetworkBehaviour
{
    [SerializeField] private float shootForce = 20f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float recoilAngle = 5f;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Rigidbody rb;
    private Quaternion initialRotation;

    void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hits anything
        Destroy(gameObject);
    }

     void Start()
    {
            rb.AddForce(bulletSpawnPoint.forward * shootForce, ForceMode.Impulse);
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        // Apply recoil by rotating the pistol upward
        transform.localRotation = Quaternion.Euler(-recoilAngle, initialRotation.y, initialRotation.z);

        // Destroy the bullet after a certain time (adjust as needed)
        Destroy(gameObject, 3f);
    }
}