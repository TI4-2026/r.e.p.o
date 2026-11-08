
using UnityEngine;

public class PlatformBehavior : PlayerCollisionObject
{
    [Header("Attributes")]
    [SerializeField] protected bool rightDirection = true;
    [SerializeField] protected float distance = 5f;
    [SerializeField] protected float speed = 2f;
    public float waitTime = 2f;

    protected Rigidbody rb;
    protected Vector3 startPosition;
    protected Vector3 endPosition;
    protected Vector3 targetPosition;
    protected Vector3 previousPosition;
    protected Vector3 nextPosition;

    protected bool isPlayerAbove;
    protected PlayerMovement playerMovement;

    private float waitTimer;
    private bool isWaiting;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        startPosition = transform.position;
        previousPosition = transform.position;
        nextPosition = transform.position;

        float direction = rightDirection ? 1f : -1f;
        endPosition = startPosition + transform.right * direction * distance;

        targetPosition = endPosition;

        isPlayerAbove = false;
        playerMovement = null;
    }

    protected virtual void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        previousPosition = transform.position;

        if (isWaiting)
        {
            if (Time.time >= waitTimer)
            {
                isWaiting = false;
            }
        }
        else
        {
            nextPosition = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            rb.MovePosition(nextPosition);

            if (Vector3.Distance(transform.position, targetPosition) <= 0.001f)
            {
                nextPosition = targetPosition;
                waitTimer = Time.time + waitTime;
                isWaiting = true;

                targetPosition = targetPosition == endPosition
                    ? startPosition
                    : endPosition;
            }
        }
    }

    public override void OnPlayerCollisionEnter(GameObject player)
    {
        Debug.Log("CollisionEnter");
    }

    public override void OnPlayerCollisionExit(GameObject player)
    {
        Debug.Log("CollisionExit");
    }

    public Vector3 GetPlatformMovement()
    {
        return nextPosition - previousPosition;
    }
}
