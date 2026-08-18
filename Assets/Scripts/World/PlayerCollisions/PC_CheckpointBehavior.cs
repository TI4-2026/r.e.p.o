using UnityEngine;
using UnityEngine.Events;

public class PC_CheckpointBehavior : PlayerCollisionObject
{
    [SerializeField] private int checkpointPriority = 0;
    
    protected override void OnPlayerTriggerEnter(GameObject go)
    {
        GameManager.Instance.SetCheckpoint(this.transform, checkpointPriority);
    }
}
