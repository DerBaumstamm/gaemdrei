using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : NetworkBehaviour
{
    //movement input variables
    [SerializeField] private InputAction movementAction;
    [SerializeField] private InputAction lookAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction sprintAction;
    [SerializeField] private InputAction menuAction;

    //object variables
    [SerializeField] private EscapeUI escapeUI;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private List<Vector3> spawnPositionList;
    [SerializeField] private List<Vector3> gameOverPositionList;
    [SerializeField] private Animator animator;
    private Camera cam;

    //value variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed; 
    [SerializeField] private float lookSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckDistance; 
    [SerializeField] private float groundHeight;
    private bool isMenuShown = false;
    private bool isSprinting = false;
    private float rotationX;

    //vector variables
    Vector2 moveDirection;
    Vector2 lookInput;
    Vector3 cameraRotation;
    Quaternion camRotation;
    Vector3 cameraRotation2;

    //enables all movement inputs and camera
    private void OnEnable()
    {
        cam = playerCamera.GetComponent<Camera>();
        movementAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        menuAction.Enable();
        Invoke(nameof(enableCamera), .1f);
    }

    //disables all movement inputs
    private void OnDisable()
    {
        movementAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
        menuAction.Disable();
    }

    // locks cursor and spawns player with according position and rotation
    public override void OnNetworkSpawn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (SceneManager.GetActiveScene().name != Loader.Scene.GameOver.ToString())
        {
            transform.position = spawnPositionList[GameMultiplayer.Instance.getPlayerDataIndexFromClientId(OwnerClientId)];
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            transform.position = gameOverPositionList[GameMultiplayer.Instance.getPlayerDataIndexFromClientId(OwnerClientId)];
        }
        
        camRotation = Quaternion.Euler(playerCamera.transform.eulerAngles);       
    }

    //applies all rotations + triggers jump
    private void Update()
    {
        if (!IsOwner) return;
        //assign movement inputs to variables
        lookInput = lookAction.ReadValue<Vector2>();
        moveDirection = movementAction.ReadValue<Vector2>();

        //jump if player is on ground
        if (jumpAction.triggered && isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (menuAction.triggered)
        {          
            escapeUI.show();
            Cursor.lockState = CursorLockMode.None;
        }

        //set isSprinting to true when sprint button is pressed
        isSprinting = sprintAction.ReadValue<float>() > 0.5f;

        //apply mouse input to body rotation around the Y axis
        Vector3 bodyRotation = new Vector3(0, lookInput.x, 0) * lookSpeed;
        transform.rotation = transform.rotation * Quaternion.Euler(bodyRotation);

        //apply mouse input to camera rotation around the X axis (and restrict to -80|65 degrees)
        rotationX -= lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -80, 65);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Adjust speed based on sprinting state
        float speed = isSprinting ? sprintSpeed : moveSpeed;


        Vector3 desiredMoveDirection = forward * moveDirection.y + right * moveDirection.x;
        rb.velocity = new Vector3(desiredMoveDirection.x * speed, rb.velocity.y, desiredMoveDirection.z * speed);
        animator.SetFloat("animSpeed", rb.velocity.magnitude);
    }

    //applies wasd movement + sprint
    private void FixedUpdate()
    {
        //if (!IsOwner) return;

        
    }

    //Use a raycast to check if the player is grounded
    private bool isGrounded()
    {       
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + groundHeight);
    }

    //only enables camera for own playerObject
    private void enableCamera()
    {
        if (IsOwner)
        {          
            cam.enabled = true;
        }
    }
}
