using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : NetworkBehaviour
{
    [SerializeField] private InputAction lookAction;
    [SerializeField] private float lookSpeed = 0.1f;
    [SerializeField] private Camera cam;
    Vector2 lookInput = Vector2.zero;

    private void OnEnable()
    {
        lookAction.Enable();
        Invoke(nameof(enableCamera), .1f);
    }

    private void OnDisable()
    {       
        lookAction.Disable();
    }

    void Update()
    {
        if (!IsOwner) return;
        lookInput = lookAction.ReadValue<Vector2>();       
        Vector3 rotation = new Vector3(-lookInput.y, 0f, 0f) * lookSpeed;
        transform.rotation = transform.rotation * Quaternion.Euler(rotation);   
    }

    private void enableCamera()
    {
        if (IsOwner)
        {
            cam.enabled = true;
        }
    }
}
