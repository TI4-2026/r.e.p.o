using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Singleton instance
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

    // ============= Variables =============

    [Header("Attributes")]
    [SerializeField] private Transform spawnpoint;

    private Transform checkpoint = null;
    private int currentCheckpointPriority = 0;

    // ============= Private Methods =============



    // ============= Public Methods =============

    public void SpawnPlayer(GameObject player)
    {
        Transform teleportPoint = checkpoint != null ? checkpoint : spawnpoint;

        player.GetComponent<PlayerMovement>().Teleport(teleportPoint);
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
