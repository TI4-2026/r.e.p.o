using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
            return;
        }

        if (Hud == null)
        {
            Hud = FindFirstObjectByType<Hud>();
        }
    }

    // --------------- Variables ---------------

    [Header("Attributes")]
    [SerializeField] private Transform spawn;

    [Header("Autofilled")]
    public Hud Hud;
    public Transform checkpoint = null;
    private TransitionSceneTrigger exitLevel = null;
    public GameObject player;
    
    private int currentCheckpointPriority = 0;
    private int gearsCollected = 0;
    public bool IsRespawning { get; private set; }

    // --------------- Public Methods ---------------

    public void CollectGear(bool enableExit = false)
    {
        gearsCollected++;
        if (enableExit) EnableExit();
    }
    
    public void SpawnPlayer(GameObject player, bool shownBlackFade = false)
    {
        Transform teleportPoint = checkpoint != null ? checkpoint : spawn;
        GameObject targetPlayer = player != null
            ? player
            : (this.player != null ? this.player : GameObject.FindGameObjectWithTag("Player"));

        if (targetPlayer == null || !targetPlayer.TryGetComponent(out PlayerMovement playerMovement))
        {
            Debug.LogWarning("GameManager.SpawnPlayer: Player or PlayerMovement not found.");
            return;
        }

        if (teleportPoint == null)
        {
            Debug.LogWarning("GameManager.SpawnPlayer: No checkpoint or spawn Transform assigned.");
            playerMovement.SetMovementEnabled(true);
            return;
        }

        if (IsRespawning) return;

        if (shownBlackFade && Hud != null)
        {
            IsRespawning = true;
            playerMovement.SetMovementEnabled(false);

            Hud.BlackFade(
                onMiddle: () =>
                {
                    playerMovement.SetMovementEnabled(false);
                    playerMovement.ExecuteTeleport(teleportPoint);
                },
                onComplete: () =>
                {
                    IsRespawning = false;
                    playerMovement.SetMovementEnabled(true);
                }
            );
        }
        else
        {
            playerMovement.ExecuteTeleport(teleportPoint);
            playerMovement.SetMovementEnabled(true);
        }
    }

    public void KillPlayer(GameObject player, bool showBlackFade = false)
    {
        SpawnPlayer(player, showBlackFade);
        
    }

    public void SetCheckpoint(Transform newCheckpoint, int priority=0)
    {
        if (priority < currentCheckpointPriority) return;

        checkpoint = newCheckpoint;
        currentCheckpointPriority = priority;
    }

    public void SetExit(TransitionSceneTrigger exit)
    {
        if (exitLevel != null) return;
        
        exitLevel = exit;
    }
    
    // --------------- Private Methods ---------------
    
    private void EnableExit()
    {
        if (exitLevel == null) return;
        
        exitLevel.SetEnabled(true);
    }
}
