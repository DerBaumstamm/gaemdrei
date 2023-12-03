using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using System;

public class PistolShoot : NetworkBehaviour
{
    [SerializeField] private InputAction shootAction;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;      
    [SerializeField] private List<GameObject> spawnedBullets = new();
    [SerializeField] private TMP_Text scoreUi;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private void OnEnable()
    {
        shootAction.Enable();
        
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    //enables and updates leaderboard if player is owner
    private void Start()
    {
        if (IsLocalPlayer)
        {
            scoreUi.enabled = true;
            scoreUi.text = GameMultiplayer.Instance.GetLeaderboard();
        }
    }

    //checks if player shot
    void Update()
    {
        if(!IsOwner) return;

        if (shootAction.triggered && Time.time >= nextFireTime)
        {
            ShootServerRpc();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    //sends request to server to spawn bullet
    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject bulletPrefabTransform = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        spawnedBullets.Add(bulletPrefabTransform);                          //adds bullet to list of currently existing bullets
        bulletPrefabTransform.GetComponent<BulletBehavior>().parent = this; //ties bullet to this player
        bulletPrefabTransform.GetComponent<NetworkObject>().Spawn();        //initiates network object of the bullet
    }

    //sends request to server to despawn bullet
    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedBullets[0];            //takes oldest bullet from exisitng bullet list
        toDestroy.GetComponent<NetworkObject>().Despawn();   //destroys network objct of bullet
        if(toDestroy != null)
        {
            spawnedBullets.Remove(toDestroy);                //removes bullet from list
            Destroy(toDestroy);                              //destroys bullet object
        }
                                        
    }

    //updates score in Playerdata of the player who shot the bullet
    public void updatePlayerScore()
    {
        GameMultiplayer.Instance.AddPlayerScore(OwnerClientId);
        GameManager.Instance.updateLeaderboard();                   
    }

    public int getClientId()
    {
        PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        return Convert.ToInt32(playerData.clientId);
    }
}
