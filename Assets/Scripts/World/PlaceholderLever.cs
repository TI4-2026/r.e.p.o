using UnityEngine;

public class PlaceholderLever : PlayerCollisionObject
{
    public GameObject door;
    public Animator Lever;
    protected override void OnPlayerTriggerEnter(GameObject player)
    {
        door.SetActive(false);
        Lever.SetTrigger("Ativar");
    }
    
}
