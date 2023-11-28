using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : NetworkBehaviour
{
    [SerializeField] private InputAction movementAction;
    [SerializeField] private InputAction lookAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction sprintAction;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private List<Vector3> spawnPositionList;
    [SerializeField] private Animator animator;
    private Camera cam;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f; 
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 0.2f; 
    [SerializeField] private float groundHeight = 0.5f;
    [SerializeField] private bool isSprinting = false;

   

    Vector2 moveDirection = Vector2.zero;
    Vector2 lookInput = Vector2.zero;

    private void OnEnable()
    {
        cam = playerCamera.GetComponent<Camera>();
        movementAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        Invoke(nameof(enableCamera), .1f);
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
        transform.position = spawnPositionList[GameMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];      
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


        Vector3 bodyRotation = new Vector3(0, lookInput.x, 0) * lookSpeed;
        transform.rotation = transform.rotation * Quaternion.Euler(bodyRotation);
        Vector3 cameraRotation = new Vector3(-lookInput.y, 0, 0) * lookSpeed;
        playerCamera.transform.rotation = playerCamera.transform.rotation * Quaternion.Euler(cameraRotation);

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
        animator.SetFloat("animSpeed",rb.velocity.magnitude);
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

    private void enableCamera()
    {
        if (IsOwner)
        {
            cam.enabled = true;
        }
    }
}
