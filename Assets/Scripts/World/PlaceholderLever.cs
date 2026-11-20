using UnityEngine;

public class PlaceholderLever : PlayerCollisionObject
{
    public GameObject door;
    protected override void OnPlayerTriggerEnter(GameObject player)
    {
        door.SetActive(false);
    }
    
}
