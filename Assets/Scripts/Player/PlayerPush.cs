using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushForce = 5f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        Vector3 pushDirection = new Vector3(
            hit.moveDirection.x,
            0f,
            hit.moveDirection.z
        );

        rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
    }
}
