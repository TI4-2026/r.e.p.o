using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController characterController;
    private Vector3 verticalVelocity;
    private Vector2 moveInput;
    private bool jumpRequested;
    private float groundCheckDistance = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMovement();
        VerticalMovement();
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

    // --------------- Movement ---------------

    private void HorizontalMovement()
    {
        Vector3 movementDirection = (transform.right * moveInput.x) + (transform.forward * moveInput.y);
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);

        characterController.Move(movementDirection * speed * Time.deltaTime);
    }

    private void VerticalMovement()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        if (jumpRequested)
        {
            verticalVelocity.y = jumpForce;
            jumpRequested = false;
        }

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
