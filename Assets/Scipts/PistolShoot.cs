using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PistolShoot : NetworkBehaviour
{
    [SerializeField] private InputAction shootAction;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float fireRate = 0.5f;

    [SerializeField] private List<GameObject> spawnedBullets = new List<GameObject>();

    private float nextFireTime = 0f;

    private void OnEnable()
    {
        shootAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    void Update()
    {
        if(!IsOwner) return;

        if (shootAction.triggered && Time.time >= nextFireTime)
        {
            ShootServerRpc();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject bulletPrefabTransform = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        spawnedBullets.Add(bulletPrefabTransform);
        bulletPrefabTransform.GetComponent<BulletBehavior>().parent = this;
        bulletPrefabTransform.GetComponent<NetworkObject>().Spawn(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedBullets[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedBullets.Remove(toDestroy);
        Destroy(toDestroy);
    }
}
