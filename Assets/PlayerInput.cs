using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class script : MonoBehaviour
{
    public InputAction gameplayActions;
    public Rigidbody rb;
    public float moveSpeed = 5f;
    Vector3 moveDirection = Vector3.zero;

    private void OnEnable()
    {
        gameplayActions.Enable();
    }

    private void OnDisable()
    {
        gameplayActions.Disable();
    }

    private void Update()
    {
        moveDirection = gameplayActions.ReadValue<Vector3>();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, moveDirection.z * moveSpeed);
    }
}
