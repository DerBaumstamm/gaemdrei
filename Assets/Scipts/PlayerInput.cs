using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerInput : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private InputAction movementAction;
    [SerializeField] private InputAction lookAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction sprintAction;


    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform playerCamera; // Reference to the camera
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f; // Adjust the sprinting speed
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 0.2f; // Distance to check if the player is grounded
    [SerializeField] private float groundHeight = 0.5f; // Y-coordinate value for the ground level 
    [SerializeField] private bool isSprinting = false;

    Vector2 moveDirection = Vector2.zero;
    Vector2 lookInput = Vector2.zero;

    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        movementAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //audioListener.enabled = true;
            virtualCamera.Priority = 1;
        }
        else
        {
            virtualCamera.Priority = 0;
        }
    }
    private void Update()
    {
        if (!IsOwner) return;

        moveDirection = movementAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        if (jumpAction.triggered && IsGrounded())
        {
            Jump();
        }
        isSprinting = sprintAction.ReadValue<float>() > 0.5f;

        RotatePlayer();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Adjust speed based on sprinting state
        float speed = isSprinting ? moveSpeed : sprintSpeed;

        Vector3 desiredMoveDirection = forward * moveDirection.y + right * moveDirection.x;
        rb.velocity = new Vector3(desiredMoveDirection.x * speed, rb.velocity.y, desiredMoveDirection.z * speed);
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
