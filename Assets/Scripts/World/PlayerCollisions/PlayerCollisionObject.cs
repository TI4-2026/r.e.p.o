using UnityEngine;

public class PlayerCollisionObject : MonoBehaviour
{
    /*
        Este script é usado para gerenciar colisões entre o jogador e objetos.
        O Character Controller não aciona naturalmente o OnCollisionEnter.
        Embora acione o OnTriggerEnter, esse também será gerenciado por este script.
    */


    // OnPlayerCollisionEnter é chamado a partir de PlayerCollisionSelf.cs
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
