using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target;  // The target object to follow
    public float smoothSpeed = 0.125f;  // Speed at which the camera follows the target
    public float distance = 1f;  // The distance between the camera and the target
    public float height = 0.5f;  // The height above the target

    void LateUpdate()
    {
        if (target != null)
        {            
            Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // You can also make the camera look at the target if needed
            transform.LookAt(target.position);
        }
    }
}
