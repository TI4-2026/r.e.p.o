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

    // --------------- Public Methods ---------------

    public void CollectGear(bool enableExit = false)
    {
        gearsCollected++;
        if (enableExit) EnableExit();
    }
    
    public void SpawnPlayer(GameObject player)
    {
        Transform teleportPoint = checkpoint != null ? checkpoint : spawn;

        PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if (Hud != null)
        {
            Hud.BlackFade(
                onMiddle: () => 
                {
                    playerMovement.SetMovementEnabled(false);
                    playerMovement.ExecuteTeleport(teleportPoint);
                },
                onComplete: () => playerMovement.SetMovementEnabled(true)
            );
        }
        else
        {
            playerMovement.ExecuteTeleport(teleportPoint);
        }
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
