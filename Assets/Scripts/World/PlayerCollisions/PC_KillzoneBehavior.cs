using UnityEngine;

public class PC_KillZoneBehavior : PlayerCollisionObject
{
    protected override void OnPlayerTriggerEnter(GameObject go)
    {
        GameManager.Instance.KillPlayer(go);
    }
}
