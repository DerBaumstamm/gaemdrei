using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BulletBehavior : NetworkBehaviour
{
    public PistolShoot parent; //script which belongs to a player with playerID to send scoreUpdate to
    [SerializeField] private float shootForce = 20f;
    [SerializeField] private Transform bulletSpawnPoint;
    private Rigidbody rb;

    //applies Force upon start
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(bulletSpawnPoint.forward * shootForce, ForceMode.Impulse);       
    }

    //updates playerscore and destroys bullet for players and server upon impact with a player
    private void OnTriggerEnter(Collider collider)
    {
        if (!IsOwner) return;
        if(collider.tag == "Player")
        {           
            parent.updatePlayerScoreServerRpc();                     
        }
        parent.destroyServerRpc();
    }
}