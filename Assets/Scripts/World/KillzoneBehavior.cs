using UnityEngine;

public class KillZoneBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player killed!");
            GameManager.Instance.KillPlayer(other.gameObject);
        }
    }
}
