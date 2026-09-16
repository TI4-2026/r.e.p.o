using UnityEngine;

public class PC_KillZoneBehavior : PlayerCollisionObject
{
    protected override void OnPlayerTriggerEnter(GameObject go)
    {
        PlayerLife playerLife = go.GetComponent<PlayerLife>();
        playerLife.TakeDamage(10f);
        GameManager.Instance.KillPlayer(go);
    }
}
