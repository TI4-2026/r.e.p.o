using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed = 5f;
    private float defaultSpeed;
    [SerializeField] private float acceleration = 0.15f;
    [SerializeField] private float deceleration = 0.1f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float maxFallSpeed = -20f;
     [SerializeField]private float slideFriction = 20f;
     [SerializeField]private float slideSpeed = 3f;

    [Header("Settings")]
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float isGroundedGraceTime = 0.2f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpHeldGravityMultiplier = 3f;

    [Header("Ledge Grab")]
    [SerializeField] private LayerMask ledgeGrabLayers;
    [SerializeField] private float ledgeGrabForwardReach = 0.9f;
    [SerializeField] private float ledgeGrabMinHeight = 0.15f;
    [SerializeField] private float ledgeGrabMaxHeight = 1.35f;
    [SerializeField] private float ledgeGrabVerticalBoost = 14f;
    [SerializeField] private float ledgeGrabForwardBoost = 3f;
    [SerializeField] private float ledgeGrabTopInset = 0.12f;

    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerCollisionSelf playerCollisionSelf = null;
     private PlayerInteraction playerInteraction;
    
    [Header("Move Object Collision Check")]
    [SerializeField] private LayerMask moveObjectObstacleLayers = ~0;
    [SerializeField] private float boxSkinWidth = 0.02f;
    private GameObject currentMoveObject;
    private Collider currentMoveCollider;
    private BoxCollider currentBoxCollider;
    private readonly RaycastHit[] boxCastHits = new RaycastHit[8];


    // ---------- Control Variables ----------

    private Vector3 verticalVel;
    private Vector3 horizontalVel;
    private Vector3 horizontalVelRef;
    public Vector3 velocity;
    private Vector2 moveInput;
    private bool jumpRequested;
    private bool jumpButtonHeld;
    private bool isJumping;
    private bool isMovementEnabled = true;
    private bool isMoveObjectMode = false;
    private bool isGrounded;
    private bool ledgeGrabAvailable = true;
    private float jumpRequestTimer = 0f;
    private float lastGroundedTime = 0f;
    private float lastJumpTime = 0f;
    private bool isSliding = false;
    GameObject activePlatform;
    PlatformBehavior platBehave;

    // Animator
    public Animator animator;

    

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInteraction = GetComponent<PlayerInteraction>();
        mainCamera = Camera.main;
        defaultSpeed = speed;   
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
        TryLedgeGrab();
        animator.SetFloat("Speed Y", verticalVel.y + 2.36f);

        velocity = horizontalVel + verticalVel;
        Vector3 moveStep = velocity * Time.deltaTime;

        // "Pos-processamento" do movimento do player quando ele estiver movendo um objeto
        if (isMoveObjectMode)
        {
            Vector3 horizStep = new Vector3(moveStep.x, 0f, moveStep.z);
            horizStep = AdjustMovementForBox(horizStep);
            horizontalVel = horizStep / Time.deltaTime;
            moveStep = new Vector3(horizStep.x, moveStep.y, horizStep.z);
        }
            
        characterController.Move(moveStep);
        playerCollisionSelf.ccMoved.Invoke();
        animator.SetFloat("Speed", moveInput.magnitude);
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

    public Vector3 GetMovementDirectionMoveObjectMode()
    {
        Vector3 movementDirection = (transform.right * moveInput.x) + (transform.forward * moveInput.y);
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
        jumpButtonHeld = false;
        ledgeGrabAvailable = true;

        transform.forward = destination.forward;
        transform.position = destination.position;

        characterController.enabled = true;
    }


    public void SetMoveObjectMode(bool enabled, GameObject targetObject = null)
    {
        isMoveObjectMode = enabled;
        currentMoveObject = enabled ? targetObject : null;
        currentMoveCollider = enabled && targetObject != null ? targetObject.GetComponent<Collider>() : null;
        currentBoxCollider = currentMoveCollider as BoxCollider;
    }

    public float GetSpeed() {return speed;}
    public void SetSpeed(float speed) {this.speed = speed;}
    public void SetDefaultSpeed() {speed = defaultSpeed;}

    public bool IsSliding => isSliding;
    public bool CanSlide()
    {
        return !isSliding;
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

        if (context.started)
        {
            jumpButtonHeld = true;
            jumpRequested = true;
            jumpRequestTimer = Time.time;
        }

        if (context.canceled)
            jumpButtonHeld = false;
    }
    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.started && CanSlide())
        {
            StartSlide();
        }
        if (context.canceled && isSliding)
        {
            EndSlide();
        }
    }

    // --------------- Movement ---------------

    private void HorizontalMovement()
    {
        if (!characterController.enabled) return;

        Vector3 movementDirection = Vector3.zero;
        Vector3 targetVelocity = Vector3.zero;

        if (isMoveObjectMode)
        {
            movementDirection = GetMovementDirectionMoveObjectMode();
            targetVelocity = speed * movementDirection;
        }else
        {
            movementDirection = GetMovementDirection();
            if (movementDirection.sqrMagnitude > 0.0001f) RotateTowardsMovement(movementDirection);
            targetVelocity = transform.forward * speed * movementDirection.magnitude;
        }

        float smoothTime = targetVelocity.sqrMagnitude > horizontalVel.sqrMagnitude ? acceleration : deceleration;
        horizontalVel = Vector3.SmoothDamp(horizontalVel, targetVelocity, ref horizontalVelRef, smoothTime);

        //characterController.Move(horizontalVel * Time.deltaTime);
    }

    private void RotateTowardsMovement(Vector3 movementDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    
    private Vector3 AdjustMovementForBox(Vector3 horizStep)
    {        
        /*
    
        O metodo abaixo e tipo um "pos" processamento do movimento do player. Ele vai ser chamado no FixedUpdate quando o player estiver movendo um objeto.

        Ele basicamente vai verificar se a caixa que o player esta empurrando esta batendo em alguma coisa. Se ela estiver, ele vai modificar o movimento do player naquele frame "horizStep"
        para anular a parte do movimento que vai fazer a caixa entrar em outro objeto.

        Logo, SE VOCE NAO SABE O QUE ESTA FAZENDO, NAO MEXA NISSO

        */

        if (currentMoveCollider == null || horizStep.sqrMagnitude < 0.000001f)
            return horizStep;

        float distance = horizStep.magnitude;
        Vector3 direction = horizStep / distance;

        Vector3 center;
        Vector3 halfExtents;
        Quaternion orientation = currentMoveObject.transform.rotation;

        if (currentBoxCollider != null)
        {
            center = currentMoveObject.transform.TransformPoint(currentBoxCollider.center);
            Vector3 lossy = currentMoveObject.transform.lossyScale;
            halfExtents = Vector3.Scale(currentBoxCollider.size * 0.5f, new Vector3(Mathf.Abs(lossy.x), Mathf.Abs(lossy.y), Mathf.Abs(lossy.z)));
        }
        else
        {
            Bounds bounds = currentMoveCollider.bounds;
            center = bounds.center;
            halfExtents = bounds.extents;
        }

        // Reduz ligeiramente os extents para nao raspar no piso ou acusar falso contato inicial
        halfExtents -= new Vector3(boxSkinWidth, boxSkinWidth, boxSkinWidth);
        if (halfExtents.x <= 0f || halfExtents.y <= 0f || halfExtents.z <= 0f)
            halfExtents = currentMoveCollider.bounds.extents * 0.9f;

        center.y += boxSkinWidth * 1.5f;

        int hitCount = Physics.BoxCastNonAlloc(
            center,
            halfExtents,
            direction,
            boxCastHits,
            orientation,
            distance + boxSkinWidth * 2f,
            moveObjectObstacleLayers,
            QueryTriggerInteraction.Ignore
        );

        RaycastHit closestHit = default;
        float minDistance = float.MaxValue;
        bool hasHit = false;

        for (int i = 0; i < hitCount; i++)
        {
            Collider col = boxCastHits[i].collider;
            if (col == null) continue;
            if (col == currentMoveCollider || col.transform.root == transform.root) continue;

            if (boxCastHits[i].distance < minDistance)
            {
                minDistance = boxCastHits[i].distance;
                closestHit = boxCastHits[i];
                hasHit = true;
            }
        }

        if (hasHit)
        {
            float allowedDist = Mathf.Max(0f, closestHit.distance - boxSkinWidth);
            Vector3 allowedMove = direction * Mathf.Min(allowedDist, distance);

            Vector3 remainingMove = horizStep - allowedMove;
            Vector3 slideMove = Vector3.ProjectOnPlane(remainingMove, closestHit.normal);
            slideMove.y = 0f;

            if (slideMove.sqrMagnitude > 0.000001f)
            {
                float slideDist = slideMove.magnitude;
                Vector3 slideDir = slideMove / slideDist;

                int slideHitCount = Physics.BoxCastNonAlloc(
                    center + allowedMove,
                    halfExtents,
                    slideDir,
                    boxCastHits,
                    orientation,
                    slideDist + boxSkinWidth * 2f,
                    moveObjectObstacleLayers,
                    QueryTriggerInteraction.Ignore
                );

                float minSlideDist = float.MaxValue;
                bool slideHasHit = false;

                for (int i = 0; i < slideHitCount; i++)
                {
                    Collider col = boxCastHits[i].collider;
                    if (col == null) continue;
                    if (col == currentMoveCollider || col.transform.root == transform.root) continue;

                    if (boxCastHits[i].distance < minSlideDist)
                    {
                        minSlideDist = boxCastHits[i].distance;
                        slideHasHit = true;
                    }
                }

                if (slideHasHit)
                {
                    float allowedSlideDist = Mathf.Max(0f, minSlideDist - boxSkinWidth);
                    slideMove = slideDir * Mathf.Min(allowedSlideDist, slideDist);
                }
            }

            return allowedMove + slideMove;
        }

        return horizStep;
    }

    private void VerticalMovement()
    {
        if (!characterController.enabled) return;

        // Stand Gravity
        if (isGrounded && verticalVel.y < 0f)
        {
            verticalVel.y = -2f;
        }

        // Fall Gravity (vel negativa)
        if (verticalVel.y < maxFallSpeed) verticalVel.y = maxFallSpeed;

        if (isMoveObjectMode && Math.Abs(verticalVel.y) > 3f) playerInteraction.TryMoveObject();

        // Jump
        if (jumpRequested && !isMoveObjectMode)
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
                }
                else
                {
                    jumpRequestTimer = 0f;
                    jumpRequested = false;
                }
            }
        }

        float appliedGravity = gravity;
        if (isJumping && verticalVel.y > 0f && !jumpButtonHeld)
            appliedGravity *= jumpHeldGravityMultiplier;

        verticalVel.y += appliedGravity * Time.deltaTime;
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        verticalVel.y = jumpForce;
        jumpRequestTimer = 0f;
        jumpRequested = false;
        isJumping = true;
        lastJumpTime = Time.time;
    }

    private void TryLedgeGrab()
    {
        if (!ledgeGrabAvailable || !isMovementEnabled || isMoveObjectMode || isGrounded ||
            !characterController.enabled || ledgeGrabLayers.value == 0)
            return;

        Vector3 direction = horizontalVel;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.25f)
            return;
        direction.Normalize();

        float radius = characterController.radius;
        float skin = characterController.skinWidth;
        Vector3 feet = transform.position + Vector3.up * (characterController.center.y - characterController.height * 0.5f);
        float sideReach = radius + ledgeGrabForwardReach;

        RaycastHit faceHit = default;
        bool foundFace = false;
        int faceSamples = Mathf.Clamp(Mathf.CeilToInt((ledgeGrabMaxHeight - ledgeGrabMinHeight) / Mathf.Max(radius, 0.1f)) + 1, 2, 8);
        for (int i = 0; i < faceSamples; i++)
        {
            float t = (float)i / (faceSamples - 1);
            Vector3 sideOrigin = feet + Vector3.up * Mathf.Lerp(ledgeGrabMinHeight, ledgeGrabMaxHeight, t);
            if (!Physics.Raycast(sideOrigin, direction, out RaycastHit candidate, sideReach, ledgeGrabLayers, QueryTriggerInteraction.Ignore))
                continue;
            if (Vector3.Dot(candidate.normal, -direction) < 0.5f)
                continue;

            faceHit = candidate;
            foundFace = true;
            break;
        }

        if (!foundFace)
            return;

        Vector3 topProbe = faceHit.point + direction * ledgeGrabTopInset;
        topProbe.y = feet.y + ledgeGrabMaxHeight + 0.05f;
        float topProbeDistance = ledgeGrabMaxHeight - ledgeGrabMinHeight + 0.1f;
        if (!Physics.Raycast(topProbe, Vector3.down, out RaycastHit topHit, topProbeDistance, ledgeGrabLayers, QueryTriggerInteraction.Ignore))
            return;

        float ledgeHeight = topHit.point.y - feet.y;
        if (ledgeHeight < ledgeGrabMinHeight || ledgeHeight > ledgeGrabMaxHeight)
            return;

        Vector3 landingPosition = transform.position + direction * (radius + ledgeGrabTopInset);
        landingPosition.y += ledgeHeight + skin;
        if (!HasLedgeLandingClearance(landingPosition, faceHit.collider, topHit.collider))
            return;

        ledgeGrabAvailable = false;
        isGrounded = false;
        isJumping = true;
        lastJumpTime = Time.time;
        jumpRequested = false;
        jumpRequestTimer = 0f;
        verticalVel.y = ledgeGrabVerticalBoost;
        horizontalVel = direction * Mathf.Max(horizontalVel.magnitude, ledgeGrabForwardBoost);
    }

    private bool HasLedgeLandingClearance(Vector3 landingPosition, Collider faceCollider, Collider topCollider)
    {
        float radius = Mathf.Max(0.01f, characterController.radius - characterController.skinWidth);
        float halfSegment = Mathf.Max(0f, characterController.height * 0.5f - characterController.radius);
        Vector3 center = landingPosition + characterController.center;
        Vector3 bottom = center - Vector3.up * halfSegment;
        Vector3 top = center + Vector3.up * halfSegment;
        Collider[] overlaps = Physics.OverlapCapsule(bottom, top, radius, ~0, QueryTriggerInteraction.Ignore);

        foreach (Collider overlap in overlaps)
        {
            if (overlap == null || overlap == faceCollider || overlap == topCollider || overlap.transform.root == transform.root)
                continue;
            return false;
        }

        return true;
    }
    private float slideTol = 0.001f;
    private void StartSlide()
    {
        isSliding = true;
        SetMovementEnabled(false);

        deceleration *= slideFriction;
        speed += slideSpeed;
        //a operação desse if não ta funcionando, e eu não sei como fazer ele funcionar
        if (MathF.Abs(velocity.x) < slideTol && MathF.Abs(velocity.z) < slideTol)
            EndSlide();

    }
    private void EndSlide()
    {
        isSliding = false;
        deceleration /=slideFriction;
        speed -= slideSpeed;
        SetMovementEnabled(true);
    }

    private void CheckGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance))
        {
            if (Time.time - lastJumpTime < isGroundedGraceTime) return;

            isJumping = false;
            isGrounded = true;
            ledgeGrabAvailable = true;
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
