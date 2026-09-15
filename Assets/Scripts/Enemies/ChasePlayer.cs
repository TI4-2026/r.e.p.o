using UnityEngine;

public abstract class ChasePlayer : MonoBehaviour
{
    [Header("Detectar")]
    [SerializeField] protected Transform player;
    [SerializeField] protected float detectionRange = 2f;

    [Header("Movimento")]
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected bool canFly = false;

    [Header("Antiqueda")]
    private float groundCheckDistance = 1f;
    private float groundCheckHeight = 0.5f;
    [SerializeField] private LayerMask groundLayer;


    protected Rigidbody rb;
    protected bool isSpawning = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    protected virtual void FixedUpdate()
    {
        if (isSpawning)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            Vector3 direction = GetDirectionToPlayer();

            LookAtPlayer(direction);
            Move(direction);
        }
        else
        {
            StopMovement();
        }
    }

    protected Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        return direction.normalized;
    }

    protected virtual void LookAtPlayer(Vector3 direction)
    {
        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        rb.MoveRotation(Quaternion.Slerp(rb.rotation,targetRotation,rotationSpeed * Time.fixedDeltaTime)
        );
    }

    protected virtual void Move(Vector3 direction)
    {
        Vector3 velocity = direction * moveSpeed;
        if (!canFly)
        {
            if (!HasGroundAhead(direction))
            {
                StopMovement();
                return;
            }
            direction.y = 0f;
        }
        rb.linearVelocity = new Vector3(velocity.x,rb.linearVelocity.y,velocity.z);
    }

    protected virtual void StopMovement()
    {
        rb.linearVelocity = new Vector3(0f,rb.linearVelocity.y,0f);
    }

    private bool HasGroundAhead(Vector3 direction)
    {
        Vector3 origin = transform.position + direction * groundCheckDistance + Vector3.up * groundCheckHeight;
        float rayLength = groundCheckHeight + 2f;

        return Physics.Raycast(origin, Vector3.down, rayLength, groundLayer);
    }
}
