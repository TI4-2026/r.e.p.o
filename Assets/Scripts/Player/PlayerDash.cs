using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]

public class PlayerDash : MonoBehaviour
{

    [Header("Atributos")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Referencias")]
    [SerializeField] private Sandevistan sandevistanActive;
    [SerializeField] private AudioClip sound;
    [SerializeField] private Animator animator;
    private CharacterController characterController;
    private PlayerMovement playerMovement;

    // ---------- Control Variables ----------
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;
    private Vector3 dashDirection;
    private float dashElapsed;
    private float dashDuration;
    private float dashStartSpeed;
    private float dashEndSpeed;
    public bool IsDashing => isDashing;
    public float CooldownRemaining => Mathf.Max(0f, dashCooldown - (Time.time - lastDashTime));
    public float CooldownProgressNormalized => dashCooldown <= 0f ? 1f : Mathf.Clamp01((Time.time - lastDashTime) / dashCooldown);

    // --------------- Input Actions ---------------

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && CanDash())
        {
            StartDash();
            MusicManager.PlaySound(sound, gameObject.transform.position);
        }
    }

    // --------------- Default Methods ---------------

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if (!isDashing) return;

        if (!characterController.enabled)
        {
            CancelDash();
            return;
        }

        dashElapsed += Time.fixedDeltaTime;
        float t = dashDuration > 0f ? Mathf.Clamp01(dashElapsed / dashDuration) : 1f;
        float currentSpeed = Mathf.Lerp(dashStartSpeed, dashEndSpeed, t);
        playerMovement.SetExternalHorizontalVelocity(dashDirection * currentSpeed);

        if (t >= 1f)
        {
            EndDash();
        }
    }

    // --------------- Public Methods ---------------

    public bool CanDash()
    {
        return (playerMovement == null || playerMovement.isMovementEnabled) && !isDashing && CooldownRemaining <= 0f;
    }

    public void SetSandevistan(Sandevistan sand)
    {
        sandevistanActive = sand;
    }

    public void CancelDash()
    {
        if (!isDashing) return;

        isDashing = false;
        if (animator != null) animator.SetBool("dashing", false);
        if (playerMovement != null) playerMovement.ClearExternalHorizontalVelocity(preserveRunMomentum: false);
    }

    // --------------- Private Methods ---------------

    private void StartDash()
    {
        if (!characterController.enabled || Time.timeScale == 0f) return;

        dashDirection = GetDashDirection();
        Vector3 lookDirection = dashDirection;
        lookDirection.y = 0f;
        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            transform.LookAt(transform.position + lookDirection);
        }

        float runSpeed = playerMovement.GetSpeed();
        dashStartSpeed = Mathf.Max(dashSpeed * 2f, runSpeed * 1.5f);
        dashEndSpeed = playerMovement.HasMoveInput ? runSpeed : 0f;

        float averageSpeed = Mathf.Max(0.1f, (dashStartSpeed + dashEndSpeed) * 0.5f);
        dashDuration = dashDistance / averageSpeed;
        dashElapsed = 0f;

        isDashing = true;
        lastDashTime = Time.time;

        animator.SetBool("dashing", true);
        sandevistanActive.ActivateTrailExternally();

        playerMovement.SetMovementEnabled(false);
        playerMovement.SetExternalHorizontalVelocity(dashDirection * dashStartSpeed);
    }

    private void EndDash()
    {
        isDashing = false;
        animator.SetBool("dashing", false);
        playerMovement.SetMovementEnabled(true);
        playerMovement.ClearExternalHorizontalVelocity(preserveRunMomentum: true);
    }

    private Vector3 GetDashDirection()
    {
        Vector3 inputDirection = playerMovement.GetMovementDirection();
        inputDirection.y = 0f;

        if (inputDirection.sqrMagnitude <= 0.0001f)
        {
            inputDirection = Camera.main.transform.forward;
            inputDirection.y = 0f;
        }

        if (inputDirection.sqrMagnitude <= 0.0001f)
        {
            inputDirection = transform.forward;
            inputDirection.y = 0f;
        }

        return inputDirection.normalized;
    }

}
