using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushForce = 1f;
    public float maxPushSpeed = 3f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z).normalized;

        if (Vector3.Dot(rb.linearVelocity, pushDirection) < maxPushSpeed)
        {
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}