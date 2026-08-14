using UnityEngine;
using UnityEngine.Events;

public class CheckpointBehavior : MonoBehaviour
{
    [SerializeField] private int checkpointPriority = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.SetCheckpoint(this.transform, checkpointPriority);
        }
    }
}
