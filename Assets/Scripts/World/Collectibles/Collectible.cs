using UnityEngine;

public class Collectible : PlayerCollisionObject, ICollectible
{
    public int id;
    private bool isCollected;

    public int area;

    public void Collect(GameObject player)
    {
        if (isCollected)
            return;

        CollectibleManager.RegisterCollectible(id, area);

        gameObject.SetActive(false);
    }

    public void SetCollected(bool value)
    {
        isCollected = value;
        gameObject.SetActive(!value);
    }

    public override void OnPlayerCollisionEnter(GameObject player)
    {
        Collect(player);
    }

    protected override void OnPlayerTriggerEnter(GameObject player)
    {
        Collect(player);
    }
}