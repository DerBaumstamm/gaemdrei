using Unity.Netcode;
using UnityEngine;

public class BulletBehavior : NetworkBehaviour
{
    public PistolShoot parent;
    [SerializeField] private float shootForce = 20f;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject hitParticles;
    //[SerializeField] private LayerMask playerLayer;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(bulletSpawnPoint.forward * shootForce, ForceMode.Impulse);       
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!IsOwner) return;
        if(collider.tag == "Player")
        {           
            parent.updatePlayerScore();
            Debug.Log("hit player");          
        }
        else
        {
            Debug.Log("no hit player");
        }
        //instantiateParticleServerRpc();
        parent.DestroyServerRpc();
        //Destroy(gameObject);
    }

    [ServerRpc]
    private void instantiateParticleServerRpc()
    {
        GameObject hitImpact = Instantiate(hitParticles, transform.position, Quaternion.identity);
        hitImpact.GetComponent<NetworkObject>().Spawn();
        hitImpact.transform.localEulerAngles = new Vector3 (0f, 0f, -90f);
    }

}