using UnityEngine;

public class PlayerCollisionObject : MonoBehaviour
{
    /*
        This script is used to manage collisions between the player and objects.
        The Character Controller does not naturally trigger OnCollisionEnter.
        Although it does trigger OnTriggerEnter, it will also be managed by this script.
    */


    // OnPlayerCollisionEnter is called from PlayerCollisionSelf.cs
    public virtual void OnPlayerCollisionEnter(GameObject go) {}

    // ------------------------------------------------------------

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerTriggerEnter(other.gameObject);
        }
    }

    protected virtual void OnPlayerTriggerEnter(GameObject go) {}
}
