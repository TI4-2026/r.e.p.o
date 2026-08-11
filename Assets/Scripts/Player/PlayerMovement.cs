using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float maxFallSpeed = -20f;

    private CharacterController characterController;
    private Camera mainCamera;
    private Vector3 verticalVelocity;
    private Vector2 moveInput;
    private bool jumpRequested;
    private bool isSprinting;
    private float groundCheckDistance = 0.1f;
    private bool isMovementEnabled = true;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
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

        float currentSpeed = isSprinting ? sprintSpeed : speed;
        currentSpeed = !IsGrounded() ? speed : currentSpeed;
        
        characterController.Move(movementDirection * currentSpeed * Time.deltaTime);
    }

    private void FollowCameraRotation()
    {
        float cameraYaw = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, cameraYaw, 0f);
    }

    private void VerticalMovement()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        if (verticalVelocity.y < maxFallSpeed)
        {
            verticalVelocity.y = maxFallSpeed;
        }

        if (isGrounded && jumpRequested)
        {
            verticalVelocity.y = jumpForce;
        }

        jumpRequested = false;
        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance))
        {
            return true;
        }
        return false;
    }
}
