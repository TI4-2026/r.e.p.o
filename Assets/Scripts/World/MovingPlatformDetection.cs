using UnityEngine;

public class MovingPlatformDetection : PlayerCollisionObject
{
    public GameObject platform;
    PlayerMovement playerMovement;
    void Start()
    {
    }
    protected override void OnPlayerTriggerEnter(GameObject player)
    {
        Debug.Log("ta na plataforma");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.StartFollowing(platform);
    }
    protected override void OnPlayerTriggerExit(GameObject player)
    {
        Debug.Log("saiu da plataforma");
        if (playerMovement != null)
        {
            playerMovement.StopFollowing();
        }
    }
}
