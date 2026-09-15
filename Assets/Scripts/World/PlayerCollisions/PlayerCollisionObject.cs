using UnityEngine;

public class PlayerCollisionObject : MonoBehaviour
{
    /*
        This script is used to manage collisions between the player and objects.
        The Character Controller does not naturally trigger OnCollisionEnter.
        Although it does trigger OnTriggerEnter, it will also be managed by this script.
    */


    // OnPlayerCollisionEnter is called from PlayerCollisionSelf.cs
    public virtual void OnPlayerCollisionEnter(GameObject player) {}
    public virtual void OnPlayerCollisionStay(GameObject player) {}
    public virtual void OnPlayerCollisionExit(GameObject player) {}

    // ------------------------------------------------------------

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerTriggerEnter(other.gameObject);
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerTriggerStay(other.gameObject);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerTriggerExit(other.gameObject);
        }
    }

    protected virtual void OnPlayerTriggerEnter(GameObject player) {}
    protected virtual void OnPlayerTriggerStay(GameObject player) {}
    protected virtual void OnPlayerTriggerExit(GameObject player) {}
}
