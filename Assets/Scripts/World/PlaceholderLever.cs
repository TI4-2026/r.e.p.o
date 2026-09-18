using UnityEngine;

public class PlaceholderLever : PlayerCollisionObject
{
    public GameObject door;
    protected override void OnPlayerTriggerEnter(GameObject player)
    {
        Debug.Log("Puxou a alavanca");
        door.SetActive(false);
    }
    
}
