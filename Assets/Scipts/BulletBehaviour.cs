using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits anything
        Destroy(gameObject);
    }
}