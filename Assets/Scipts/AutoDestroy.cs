using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AutoDestroy : NetworkBehaviour
{
    public float delayBeforeDestroy = 5f;

    void Start()
    {
        Invoke("destroyObjectsServerRpc", delayBeforeDestroy);
    }

    [ServerRpc(RequireOwnership = false)]
    private void destroyObjectsServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
        if(this != null)
        {
            Destroy(gameObject);
        }
       
    }
}
