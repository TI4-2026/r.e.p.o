using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float maxFallSpeed = -20f;

    [Header("Settings")]
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float isGroundedGraceTime = 0.2f;
    private CharacterController characterController;
    private Camera mainCamera;
    
    // ---------- Control Variables ----------
    private Vector3 verticalVelocity;
    private Vector2 moveInput;
    private float currentSpeed;
    private bool jumpRequested;
    private bool isSprinting;
    private bool isJumping;
    private bool isMovementEnabled = true;
    private bool isGrounded;
    private float jumpRequestTimer=0f;
    private float lastGroundedTime=0f;
    private float lastJumpTime=0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        CheckGrounded();
        if (isMovementEnabled)
        {
            FollowCameraRotation();
            HorizontalMovement();
        }
        VerticalMovement();
    }

    // --------------- Public Methods ---------------

    public void SetMovementEnabled(bool enabled)
    {
        isMovementEnabled = enabled;
    }

    // --------------- Input Actions ---------------

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpRequested = true;
            jumpRequestTimer = Time.time;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    // --------------- Movement ---------------

    private void HorizontalMovement()
    {
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraRight.y = 0f;
        cameraForward.y = 0f;
        cameraRight.Normalize();
        cameraForward.Normalize();

        Vector3 movementDirection = (cameraRight * moveInput.x) + (cameraForward * moveInput.y);
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);

        if (isGrounded)
        {
            currentSpeed = isSprinting ? sprintSpeed : speed;
        }
        characterController.Move(movementDirection * currentSpeed * Time.deltaTime);
    }

    private void FollowCameraRotation()
    {
        float cameraYaw = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, cameraYaw, 0f);
    }

    private void VerticalMovement()
    {
        // Stand Gravity
        if (isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        // Fall Gravity
        if (verticalVelocity.y < maxFallSpeed)
        {
            verticalVelocity.y = maxFallSpeed;
        }

        // Jump
        if (jumpRequested)
        {
            // Coyote Time
            if (!isGrounded && Time.time - lastGroundedTime < coyoteTime && !isJumping)
            {
                Jump();
            }
            
            // Jump and Jump Buffer
            if (isGrounded)
            {
                if (Time.time - jumpRequestTimer < jumpBufferTime) // Jump Buffer
                {
                    Jump();
                }else
                {
                    jumpRequestTimer = 0f;
                    jumpRequested = false;
                }
            }
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        verticalVelocity.y = jumpForce;
        jumpRequestTimer = 0f;
        jumpRequested = false;
        isJumping = true;
        lastJumpTime = Time.time;
    }

    private void CheckGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance))
        {
            if (Time.time - lastJumpTime < isGroundedGraceTime) return;
            
            isJumping = false;
            isGrounded = true;
            lastGroundedTime = Time.time;
        }
        else
        {
            isGrounded = false;
        }
    }
}
