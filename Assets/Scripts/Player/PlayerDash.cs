using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]

public class PlayerDash : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashCooldown = 1f;
    public Sandevistan sandevistanActive;

    [Header("SFX")]
    [SerializeField] private AudioClip sound;
    public Animator animator;

    private CharacterController characterController;
    private PlayerMovement playerMovement;

    // ---------- Control Variables ----------
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;
    public bool IsDashing => isDashing;
    public float CooldownRemaining => Mathf.Max(0f, dashCooldown - (Time.time - lastDashTime));
    public float CooldownProgressNormalized => dashCooldown <= 0f ? 1f : Mathf.Clamp01((Time.time - lastDashTime) / dashCooldown);

    // --------------- Input Actions ---------------

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && CanDash())
        {
            StartDash();
            //MusicManager.PlaySound(sound, gameObject.transform.position);
        }
    }

    // --------------- Default Methods ---------------

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // --------------- Public Methods ---------------

    public bool CanDash()
    {
        return !isDashing && CooldownRemaining <= 0f;
    }

    // --------------- Private Methods ---------------

    private void StartDash()
{
    if (!characterController.enabled) return;
    
    isDashing = true;
    lastDashTime = Time.time;

    animator.SetBool("dashing", true);
    sandevistanActive.ActivateTrailExternally();

    playerMovement.SetMovementEnabled(false);
    Vector3 lookDirection = GetDashDirection();
    lookDirection.y = 0f;
    transform.LookAt(transform.position + lookDirection);
    Vector3 direction = GetDashDirection();
    float duration = dashDistance / dashSpeed;
    float previousValue = 0f;
    LeanTween.value(gameObject, 0f, dashDistance, duration)
        .setEase(LeanTweenType.easeOutQuad)
        .setOnUpdate((float value) =>
        {
            float delta = value - previousValue;
            previousValue = value;

            try
            {
                characterController.Move(direction * delta);
            }
            catch { }
        })
        .setOnComplete(EndDash);
}

    private void EndDash()
    {
        isDashing = false;
        animator.SetBool("dashing", false);
        playerMovement.SetMovementEnabled(true);
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

        return inputDirection;
    }

}
