using UnityEngine;

public class PC_PlatformBreakable : PlayerCollisionObject
{
    private PlatormBreakableBehavior platformBreakableBehavior;

    private void Awake()
    {
        platformBreakableBehavior = GetComponent<PlatormBreakableBehavior>();
    }

    public override void OnPlayerCollisionEnter(GameObject go)
    {
        platformBreakableBehavior.OnPlayerCollision(go);
    }
}
