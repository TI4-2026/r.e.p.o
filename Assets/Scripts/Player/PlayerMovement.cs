using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float acceleration = 0.15f;
    [SerializeField] private float deceleration = 0.1f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float maxFallSpeed = -20f;

    [Header("Settings")]
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float isGroundedGraceTime = 0.2f;
    [SerializeField] private float rotationSpeed = 10f;
    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerCollisionSelf playerCollisionSelf = null;
    
    // ---------- Control Variables ----------

    private Vector3 verticalVel;
    private Vector3 horizontalVel;
    private Vector3 horizontalVelRef;
    public Vector3 velocity;
    private Vector2 moveInput;
    private bool jumpRequested;
    private bool isJumping;
    private bool isMovementEnabled = true;
    private bool isGrounded;
    private float jumpRequestTimer=0f;
    private float lastGroundedTime=0f;
    private float lastJumpTime=0f;
    GameObject activePlatform;
    PlatformBehavior platBehave;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        CheckGrounded();
        if (activePlatform != null)
        {
            FollowPlatform();
        }
        HorizontalMovement();
        VerticalMovement();

        velocity = horizontalVel + verticalVel;
            
        characterController.Move(velocity * Time.deltaTime);
        
        playerCollisionSelf.ccMoved.Invoke();
    }

    // --------------- Public Methods ---------------

    public void SetPlayerCollisionSelf(PlayerCollisionSelf script)
    {
        if (playerCollisionSelf != null) return;

        playerCollisionSelf = script;
    }

    public void SetMovementEnabled(bool enabled)
    {
        isMovementEnabled = enabled;
    }

    public Vector3 GetMovementDirection()
    {
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraRight.y = 0f;
        cameraForward.y = 0f;
        cameraRight.Normalize();
        cameraForward.Normalize();

        Vector3 movementDirection = (cameraRight * moveInput.x) + (cameraForward * moveInput.y);
        return movementDirection.normalized;
    }

    public void ExecuteTeleport(Transform destination)
    {
        characterController.enabled = false;
        
        verticalVel = Vector3.zero;
        horizontalVel = Vector3.zero;
        horizontalVelRef = Vector3.zero;
        isJumping = false;
        lastGroundedTime = 0f;
        lastJumpTime = 0f;
        jumpRequestTimer = 0f;
        jumpRequested = false;

        transform.forward = destination.forward;
        transform.position = destination.position;

        characterController.enabled = true;
    }

    // --------------- Input Actions ---------------

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isMovementEnabled)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isMovementEnabled) return;

        if (context.performed)
        {
            jumpRequested = true;
            jumpRequestTimer = Time.time;
        }
    }

    // --------------- Movement ---------------

    private void HorizontalMovement()
    {
        if (!characterController.enabled) return;

        Vector3 movementDirection = GetMovementDirection();
        if (movementDirection.sqrMagnitude > 0.0001f) RotateTowardsMovement(movementDirection);

        Vector3 targetVelocity = transform.forward * speed * movementDirection.magnitude;
        float smoothTime = targetVelocity.sqrMagnitude > horizontalVel.sqrMagnitude ? acceleration : deceleration;
        horizontalVel = Vector3.SmoothDamp(horizontalVel, targetVelocity, ref horizontalVelRef, smoothTime);

        //characterController.Move(horizontalVel * Time.deltaTime);
    }

    private void RotateTowardsMovement(Vector3 movementDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void VerticalMovement()
    {
        if (!characterController.enabled) return;
        
        // Stand Gravity
        if (isGrounded && verticalVel.y < 0f)
        {
            verticalVel.y = -2f;
        }

        // Fall Gravity
        if (verticalVel.y < maxFallSpeed)
        {
            verticalVel.y = maxFallSpeed;
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

        verticalVel.y += gravity * Time.deltaTime;
        //characterController.Move(verticalVel * Time.deltaTime);
    }

    private void Jump()
    {
        verticalVel.y = jumpForce;
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
    private void FollowPlatform()
{
    if (platBehave != null)
    {
        Debug.Log("seguindo a plataforma");
        Vector3 platformMovement = platBehave.GetPlatformMovement();

        characterController.Move(platformMovement);
    }
}
    public void StartFollowing(GameObject platform)
    {
        activePlatform = platform;
        platBehave = platform.GetComponent<PlatformBehavior>();
    }
    public void StopFollowing()
    {
        activePlatform = null;
        platBehave = null;
    }
}
