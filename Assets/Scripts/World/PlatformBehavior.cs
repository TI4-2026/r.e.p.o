using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    [SerializeField] private bool rightDirection = true;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float speed = 2f;

    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        startPosition = transform.position;

        float direction = rightDirection ? 1f : -1f;
        endPosition = startPosition + transform.right * direction * distance;

        targetPosition = endPosition;
    }

    void FixedUpdate()
    {
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

        rb.MovePosition(nextPosition);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.001f)
        {
            targetPosition = targetPosition == endPosition ? startPosition : endPosition;
        }
    }
}
