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
    [SerializeField] private GameObject playerCamera;// Reference to the camera
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f; // Adjust the sprinting speed
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 0.2f; // Distance to check if the player is grounded
    [SerializeField] private float groundHeight = 0.5f; // Y-coordinate value for the ground level 
    [SerializeField] private bool isSprinting = false;

    Vector2 moveDirection = Vector2.zero;
    Vector2 lookInput = Vector2.zero;

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
        Cursor.lockState = CursorLockMode.Locked;
        updatePositionServerRpc();          
    }
    private void Update()
    {
        if (!IsOwner) return;
        
        lookInput = lookAction.ReadValue<Vector2>();
        moveDirection = movementAction.ReadValue<Vector2>();

        if (jumpAction.triggered && isGrounded())
        {
            jump();
        }
        isSprinting = sprintAction.ReadValue<float>() > 0.5f;

        if (transform.rotation.x <= 90f)
        {
            Vector3 rotation = new Vector3(0f, lookInput.x, 0f) * lookSpeed;
            transform.rotation = transform.rotation * Quaternion.Euler(rotation);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Adjust speed based on sprinting state
        float speed = isSprinting ? moveSpeed : sprintSpeed;

        Vector3 desiredMoveDirection = forward * moveDirection.y + right * moveDirection.x;
        rb.velocity = new Vector3(desiredMoveDirection.x * speed, rb.velocity.y, desiredMoveDirection.z * speed);   
    }

    private void jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool isGrounded()
    {
        // Use a raycast to check if the player is grounded
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + groundHeight);
    }

    [ServerRpc(RequireOwnership = false)]
    private void updatePositionServerRpc()
    {      
        transform.position = new Vector3(OwnerClientId * 4, 1, 0);
    }
}
