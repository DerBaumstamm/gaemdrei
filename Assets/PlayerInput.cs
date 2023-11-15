using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public InputAction movementAction;
    public InputAction lookAction;
    public InputAction jumpAction;


    public Rigidbody rb;
    public Transform playerCamera; // Reference to the camera
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 0.2f; // Distance to check if the player is grounded
    public float groundHeight = 0.5f; // Y-coordinate value for the ground level

    Vector2 moveDirection = Vector2.zero;
    Vector2 lookInput = Vector2.zero;

    private void OnEnable()
    {
        movementAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        moveDirection = movementAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        if (jumpAction.triggered && IsGrounded())
        {
            Jump();
        }

        RotatePlayer();
    }

    private void FixedUpdate()
    {
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * moveDirection.y + right * moveDirection.x;
        rb.velocity = new Vector3(desiredMoveDirection.x * moveSpeed, rb.velocity.y, desiredMoveDirection.z * moveSpeed);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        // Use a raycast to check if the player is grounded
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + groundHeight);
    }


    private void RotatePlayer()
    {
        // Adjust the player's rotation based on mouse input
        Vector3 rotation = new Vector3(0f, lookInput.x, 0f) * lookSpeed;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }
}
