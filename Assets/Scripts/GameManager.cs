using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    // --------------- Singleton ---------------
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --------------- Variables ---------------

    [Header("Attributes")]
    [SerializeField] private Transform spawn;

    [Header("Autofilled")]
    public Hud Hud;

    private Transform checkpoint = null;
    private int currentCheckpointPriority = 0;

    // --------------- Public Methods ---------------

    public void SpawnPlayer(GameObject player)
    {
        Transform teleportPoint = checkpoint != null ? checkpoint : spawn;

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        Hud.BlackFade(
            onMiddle: () => 
            {
                playerMovement.Freeze();
                playerMovement.ExecuteTeleport(teleportPoint);
            },
            onComplete: () => playerMovement.Unfreeze()
        );
    }

    public void KillPlayer(GameObject player)
    {
        SpawnPlayer(player);
    }

    public void SetCheckpoint(Transform newCheckpoint, int priority=0)
    {
        if (priority < currentCheckpointPriority) return;

        checkpoint = newCheckpoint;
        currentCheckpointPriority = priority;
    }
}
