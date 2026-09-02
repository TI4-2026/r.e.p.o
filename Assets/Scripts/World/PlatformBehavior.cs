using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] protected bool rightDirection = true;
    [SerializeField] protected float distance = 5f;
    [SerializeField] protected float speed = 2f;

    protected Rigidbody rb;
    protected Vector3 startPosition;
    protected Vector3 endPosition;
    protected Vector3 targetPosition;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        startPosition = transform.position;

        float direction = rightDirection ? 1f : -1f;
        endPosition = startPosition + transform.right * direction * distance;

        targetPosition = endPosition;
    }

    protected virtual void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

        rb.MovePosition(nextPosition);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.001f)
        {
            targetPosition = targetPosition == endPosition ? startPosition : endPosition;
        }
    }
}
